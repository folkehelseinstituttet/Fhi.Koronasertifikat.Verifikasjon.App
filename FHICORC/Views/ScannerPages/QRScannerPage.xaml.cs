using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace FHICORC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScannerPage : ContentPage
    {
        private static int DelayBetweenContinousScans = 100;
        private static int DelayBetweenAnalyzingFrames = 100;
        private static int MinResolutionHeightThreshold = 720;
        private static ZXingScannerView _scannerView;

        private bool _hasAskedForCameraPermission = false;
        private bool _inTabbar = false;
        private FlashlightState _perviousState = FlashlightState.Default;
        private TimeSpan _focusAssistTimespan = new TimeSpan(0, 0, 2);
        private MobileBarcodeScanningOptions _scanningOptions = new MobileBarcodeScanningOptions
        {
            DelayBetweenAnalyzingFrames = DelayBetweenAnalyzingFrames,
            DelayBetweenContinuousScans = DelayBetweenContinousScans,
            PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
            TryHarder = true,
            UseNativeScanning = false,
            InitialDelayBeforeAnalyzingFrames = 0,
            CameraResolutionSelector = SelectLowestResolution,
            TryInverted = true,
        };

        public bool IsLoading { get; set; }

        public QRScannerPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<QRScannerViewModel>();
        }

        public void SetFromTabbar()
        {
            _inTabbar = true;
            ((QRScannerViewModel)BindingContext).InTabbar = _inTabbar;
        }

        public async void OnViewDisplayed()
        {
            //Initialize camera so it only turns on when the page is visible. Only in tabbar.
            if (!_inTabbar) return;
            if (_scannerView != null) return;

            if (Device.RuntimePlatform == Device.iOS)
            {
                IoCContainer.Resolve<INavigationService>().SetStatusBar(Color.Transparent, Color.White);
            }
            await CreateScannerView();
        }

        public void OnViewHidden()
        {
            //Tear down camera completely. Only in tabbar.
            if (!_inTabbar) return;
            if (_scannerView == null) return;

            DestroyScannerView();
        }

        protected override async void OnAppearing()
        {
            if (_inTabbar) return;
            if (_scannerView != null)
            {
                await (BindingContext as QRScannerViewModel)?.SetFlashlightStateAsync(_perviousState);
                _scannerView.IsAnalyzing = true;
                return;
            }

            //Initialize camera from the front page
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);
            await CreateScannerView();
            base.OnAppearing();
        }

        protected override async void OnDisappearing()
        {
            _perviousState = (BindingContext as QRScannerViewModel).IsFlashlighthOn ? FlashlightState.On : FlashlightState.Off;
            await (BindingContext as QRScannerViewModel)?.SetFlashlightStateAsync(FlashlightState.Off);

            if (_inTabbar) return;
            if (_scannerView == null) return;

            // Front page - stop analyzing but keep scanning, so the scanner does not have to restart on every
            // result or when the menu is opened. Destroy logic will be handled when back is executed.
            _scannerView.IsAnalyzing = false;
            base.OnDisappearing();
        }

        private async void OnScanResult(Result result)
        {
            await ((QRScannerViewModel)BindingContext).HandleScanResult(result);
        }

        private async Task CreateScannerView()
        {
            try
            {
                var viewModel = (QRScannerViewModel)BindingContext;
                if (viewModel.HasCameraPermissions)
                {
                    viewModel.RaisePropertyChanged(() => viewModel.HasCameraPermissions);
                    // Note: We only want to show the flashlight switch in the view, if we have both Camera and Flashlight permissions
                    await Task.Run(() => (BindingContext as QRScannerViewModel)?.CheckFlashlightPermissions() ?? Task.CompletedTask);
                    _scannerView ??= new ZXingScannerView();
                    _scannerView.OnScanResult += OnScanResult;
                    _scannerView.Options = _scanningOptions;
                    _scannerView.SetBinding(ZXingScannerView.IsTorchOnProperty, nameof(QRScannerViewModel.IsFlashlighthOn));
                    try
                    {
                        // Check that Flashlight feature is available by triggering ZXingScannerView.IsTorchOnProperty
                        ((QRScannerViewModel)BindingContext).IsFlashlightSupported = true;
                        await (BindingContext as QRScannerViewModel)?.SetFlashlightStateAsync(FlashlightState.EnabledAndOff);
                    }
                    catch (FeatureNotSupportedException)
                    {
                        // Handle not supported on device, by disabling flashlight functionality from view.
                        ((QRScannerViewModel)BindingContext).IsFlashlightSupported = false;
                        await (BindingContext as QRScannerViewModel)?.SetFlashlightStateAsync(FlashlightState.DisabledAndOff);
                    }

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        if (_scannerView == null) return;
                        ScannerContainer.Children.Add(_scannerView);
                        _scannerView.IsScanning = true;
                        _scannerView.IsAnalyzing = true;
                    });

                    Device.StartTimer(_focusAssistTimespan, () =>
                    {
                        if (_scannerView == null)
                        {
                            return false;
                        }

                        _scannerView.AutoFocus();
                        return _scannerView.IsScanning;
                    });
                }
                else
                {
                    if (_hasAskedForCameraPermission)
                    {
                        viewModel.ButtonText = "SCANNER_CAMERA_PERMISSION_BUTTON_TEXT".Translate();
                        viewModel.OpenSettingsCommand = new Command(async () =>
                        {
                            await ExecuteOnceAsync(async () =>
                            {
                                await IoCContainer.Resolve<IDeepLinkingService>().GoToAppSettings();
                            });
                        });
                        return;
                    }

                    viewModel.ButtonText = "SCANNER_CAMERA_PERMISSION_NEXT_BUTTON_TEXT".Translate();
                    viewModel.OpenSettingsCommand = new Command(async () =>
                    {
                        await ExecuteOnceAsync(async () =>
                        {
                            if (await Permissions.RequestAsync<Permissions.Camera>() == PermissionStatus.Granted)
                            {
                                _hasAskedForCameraPermission = true;
                                viewModel.RaisePropertyChanged(() => viewModel.HasCameraPermissions);
                                await Task.Run(() => (BindingContext as QRScannerViewModel)?.CheckFlashlightPermissions() ?? Task.CompletedTask);
                                await CreateScannerView();
                            }
                            else
                            {
                                _hasAskedForCameraPermission = true;
                                viewModel.RaisePropertyChanged(() => viewModel.HasCameraPermissions);
                                viewModel.ButtonText = "SCANNER_CAMERA_PERMISSION_BUTTON_TEXT".Translate();
                                viewModel.OpenSettingsCommand = new Command(async () =>
                                {
                                    await ExecuteOnceAsync(async () =>
                                    {
                                        await IoCContainer.Resolve<IDeepLinkingService>().GoToAppSettings();
                                    });
                                });
                            }
                        });
                    });
                }
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Prevents camera preview distortion. Selects the lowest resolution within the tolerance of device aspect ratio.
        /// Lowest resolution is selected as the lower the resolution, the faster QR detection should be.
        /// </summary>
        /// <param name="availableResolutions">
        /// API generated list of available camera resolutions for the scanner view.
        /// </param>
        /// <returns>
        /// Lowest resolution within tolerance.
        /// </returns>
        private static CameraResolution SelectLowestResolution(List<CameraResolution> availableResolutions)
        {
            CameraResolution result = null;
            double aspectTolerance = 0.1;
            var targetRatio = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Width;
            var targetHeight = DeviceDisplay.MainDisplayInfo.Height;
            var minDiff = double.MaxValue;

            availableResolutions
                .Where(r => Math.Abs(((double)r.Width / r.Height) - targetRatio) < aspectTolerance)
                .ForEach(
                    res =>
                    {
                        if (Math.Abs(res.Height - targetHeight) < minDiff && res.Height >= MinResolutionHeightThreshold)
                        {
                            minDiff = Math.Abs(res.Height - targetHeight);
                            result = res;
                        }
                    });

            return result;
        }

        public void DestroyScannerView()
        {
            if (_scannerView == null) return;

            _scannerView.IsScanning = false;
            _scannerView.IsAnalyzing = false;
            _scannerView.OnScanResult -= OnScanResult;
            ScannerContainer.Children.Remove(_scannerView);
            _scannerView = null;
        }

        public void DisableScannerView()
        {
            if (_scannerView == null) return;

            _scannerView.IsAnalyzing = false;
            _scannerView.OnScanResult -= OnScanResult;
        }

        public void EnableScannerView()
        {
            if (_scannerView == null) return;

            _scannerView.IsAnalyzing = true;
            _scannerView.OnScanResult += OnScanResult;
        }

        protected override bool OnBackButtonPressed()
        {
            ((QRScannerViewModel)BindingContext).BackCommand.Execute(null);
            return true;
        }

        protected async Task ExecuteOnceAsync(Func<Task> awaitableTask)
        { 
            if (IsLoading) return;
                IsLoading = true;
            try
            {
                await awaitableTask();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}