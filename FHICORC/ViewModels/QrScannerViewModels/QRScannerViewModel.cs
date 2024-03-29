﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Data;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using FHICORC.ViewModels.QrScannerViewModels;
using FHICORC.Views;
using FHICORC.Views.Menu;
using FHICORC.Views.ScannerPages;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using FHICORC.Enums;

namespace FHICORC.ViewModels
{
    public class QRScannerViewModel : BaseViewModel
    {
        private static readonly string _flashlightOnIconPath;
        private static readonly string _flashlightOffIconPath;

        private const double ScanFailureVibrationDuration = 500;
        private const double ScanSuccessVibrationDuration = 250;
        
        private readonly ITokenProcessorService _tokenProcessorService;
        private readonly IPopupService _popupService;
        private readonly IDeviceFeedbackService _deviceFeedbackService;
        private readonly IPreferencesService _preferencesService;

        private bool _inTabbar = false;
        private bool _isFlashlightEnabled;
        private bool _isTorchOn;
        private string _currentFlashlightStateIconPath = _flashlightOffIconPath;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public bool IsFlashlightSupported { get; set; }

        public bool IsFlashlightButtonEnabled
        {
            get => _isFlashlightEnabled;
            set
            {
                if (!IsFlashlightSupported)
                    return;

                _isFlashlightEnabled = value;
                OnPropertyChanged(nameof(IsFlashlightButtonEnabled));
            }
        }

        public bool IsFlashlighthOn
        {
            get => _isTorchOn;
            private set
            {
                _isTorchOn = value;
                OnPropertyChanged(nameof(IsFlashlighthOn));
            }
        }
        public string CurrentFlashlightStateIconPath
        {
            get => _currentFlashlightStateIconPath;
            private set
            {
                _currentFlashlightStateIconPath = value;
                OnPropertyChanged(nameof(CurrentFlashlightStateIconPath));
            }
        }

        private bool _hasFlashlightPermissions;
        public bool HasFlashlightPermissions
        {
            get => _hasFlashlightPermissions;
            private set
            {
                _hasFlashlightPermissions = value;
                OnPropertyChanged(nameof(HasFlashlightPermissions));
            }
        }

        static QRScannerViewModel()
        {
            _flashlightOnIconPath = App.Current.Resources["ScannerFlashlightOnIcon"].ToString();
            _flashlightOffIconPath = App.Current.Resources["ScannerFlashlightOffIcon"].ToString();
        }

        private string buttonText;
        public string ButtonText {
            get
            {
                return buttonText;
            }
            set
            {
                buttonText = value;
                RaisePropertyChanged(() => ButtonText);
            }
        }

        public static QRScannerViewModel CreateQRScannerViewModel()
        {
            return new QRScannerViewModel(
                IoCContainer.Resolve<ITokenProcessorService>(),
                IoCContainer.Resolve<IPopupService>(),
                IoCContainer.Resolve<IDeviceFeedbackService>(),
                IoCContainer.Resolve<IPreferencesService>()
            );
        }

        public QRScannerViewModel(ITokenProcessorService tokenProcessorService, 
            IPopupService popupService, 
            IDeviceFeedbackService deviceFeedbackService, 
            IPreferencesService preferencesService)
        {
            _tokenProcessorService = tokenProcessorService;
            _popupService = popupService;
            _deviceFeedbackService = deviceFeedbackService;
            _preferencesService = preferencesService;
        }

        public bool HasCameraPermissions => Permissions.CheckStatusAsync<Permissions.Camera>().Result == PermissionStatus.Granted;

        public bool InTabbar
        {
            get => _inTabbar;
            set
            {
                _inTabbar = value;
                OnPropertyChanged(nameof(InTabbar));
            }
        }

        private ICommand openSettingsCommand;
        public ICommand OpenSettingsCommand
        {
            get
            {
                return openSettingsCommand;
            }
            set
            {
                openSettingsCommand = value;
                RaisePropertyChanged(() => OpenSettingsCommand);
            }
        }

        public override ICommand BackCommand => new Command(async () =>
        {
            await ExecuteOnceAsync(async () =>
            {
                if (!InTabbar)
                {
                    if (_navigationService.FindCurrentPage() is QRScannerPage qr)
                    {
                        qr.DestroyScannerView();
                    }
                    
                    await _navigationService.PopPage();
                }
            });
        });

        public ICommand OpenMenuCommand => new Command(async () =>
        {
            await ExecuteOnceAsync(async () =>
            {
                if (!InTabbar)
                {
                    if (_navigationService.FindCurrentPage() is QRScannerPage qr)
                    {
                        qr.DestroyScannerView();
                    }

                    await _navigationService.PushPage(new MenuPage(IoCContainer.Resolve<QRScannerMenuViewModel>()));
                }
            });
        });

        public async Task HandleScanResult(Result result)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (PopupNavigation.Instance.PopupStack.Any()) return;
                if (await IsResultOpen()) return;

                TokenValidateResultModel model = await _tokenProcessorService.DecodePassportTokenToModel(result.Text);

                if (model == null) return;
                
                if (model.ValidationResult == TokenValidateResult.Valid)
                {
                    _deviceFeedbackService.Vibrate(ScanSuccessVibrationDuration);
                    _deviceFeedbackService.PlaySound(SoundKeys.VALID_SCAN_SOUND);
                    await OnScanSuccess(model);
                }
                else
                {
                    _deviceFeedbackService.Vibrate(ScanFailureVibrationDuration);
                    _deviceFeedbackService.PlaySound(SoundKeys.INVALID_SCAN_SOUND);
                    await OnScanFailure(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task OnScanSuccess(TokenValidateResultModel tokenValidateResultModel)
        {
            bool anyVaccinations = false;
            bool anyTestResults = false;
            bool anyRecovery = false;
            if (tokenValidateResultModel.DecodedModel is Core.Services.Model.EuDCCModel._1._3._0.DCCPayload cwtPayload)
            {
                anyVaccinations = cwtPayload.DCCPayloadData.DCC.Vaccinations?.Any() ?? false;
                anyTestResults = cwtPayload.DCCPayloadData.DCC.Tests?.Any() ?? false;
                anyRecovery = cwtPayload.DCCPayloadData.DCC.Recovery?.Any() ?? false;
            }
            else
            {
                if (await IsResultOpen()) return;
                await _popupService.ShowScanSuccessPopup(tokenValidateResultModel.DecodedModel);
                return;
            }

            // Should be (anyVaccinations && !anyTestResults && !anyRecovery) - once we support multi-factor
            if (anyVaccinations)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await IsResultOpen()) return;
                    await _navigationService.PushPage(new ScanEuVaccineResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, tokenValidateResultModel);
                });
            }
            // Should be (!anyVaccinations && anyTestResults && !anyRecovery) - once we support multi-factor
            else if (anyTestResults)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await IsResultOpen()) return;
                    await _navigationService.PushPage(new ScanEuTestResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, tokenValidateResultModel);
                });
            }
            // Should be (!anyVaccinations && !anyTestResults && anyRecovery) - once we support multi-factor
            else if (anyRecovery)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await IsResultOpen()) return;
                    await _navigationService.PushPage(new ScanEuRecoveryResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, tokenValidateResultModel);
                });
            }
            else
            {
                await OnScanFailure(new TokenValidateResultModel() { ValidationResult = TokenValidateResult.Invalid });
            }
        }

        private async Task OnScanFailure(TokenValidateResultModel model)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                if (await IsResultOpen()) return;
                await _navigationService.PushPage(new ScannerErrorPage(), true, Enums.PageNavigationStyle.PushModallyFullscreen, model);
            });
        }

        private async Task<bool> IsResultOpen()
        {
            return await _navigationService.FindCurrentPageAsync() is IScanResultView;
        }

        public async Task<bool> CheckFlashlightPermissions()
        {
            var hasPermissions = await Permissions.CheckStatusAsync<Permissions.Flashlight>() == PermissionStatus.Granted;
            HasFlashlightPermissions = hasPermissions;
            return hasPermissions;
        }

        public ICommand ToggleFlashlight => new Command(async () =>
        {
            if (await CheckFlashlightPermissions() && IsFlashlightSupported)
                await SetFlashlightStateAsync(IsFlashlighthOn ? FlashlightState.Off : FlashlightState.On);
        });

        public Task SetFlashlightStateAsync(FlashlightState state) => Device.InvokeOnMainThreadAsync(async () =>
        {
            if (state.HasFlag(FlashlightState.On))
            {
                if (Device.RuntimePlatform is Device.iOS)
                {
                    await Flashlight.TurnOnAsync();
                }

                CurrentFlashlightStateIconPath = _flashlightOnIconPath;
                IsFlashlighthOn = true;
            }
            if (state.HasFlag(FlashlightState.Off))
            {
                if (Device.RuntimePlatform is Device.iOS)
                {
                    await Flashlight.TurnOffAsync();
                }
                CurrentFlashlightStateIconPath = _flashlightOffIconPath;
                IsFlashlighthOn = false;
            }
            if (state.HasFlag(FlashlightState.Disabled))
            {
                IsFlashlightButtonEnabled = false;
            }
            if (state.HasFlag(FlashlightState.Enabled))
            {
                IsFlashlightButtonEnabled = true;
            }
        });
    }
}