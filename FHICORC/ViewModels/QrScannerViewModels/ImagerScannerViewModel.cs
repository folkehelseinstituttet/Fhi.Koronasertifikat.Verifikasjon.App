using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using FHICORC.Configuration;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Enums;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Scanner;
using FHICORC.Services.Status;
using FHICORC.Services.Translator;
using FHICORC.ViewModels.Base;
using FHICORC.Views.ScannerPages;
using Xamarin.Forms;
using FHICORC.Views.Menu;
using FHICORC.Data;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ImagerScannerViewModel : BaseViewModel
    {
        private readonly ITokenProcessorService _tokenProcessorService;

        private bool _handlingResult = false;
        private IScannerFactory _scannerFactoryService;
        private readonly IImagerScanner _scanner;
        private readonly IPopupService _popupService;
        private readonly IDeviceFeedbackService _deviceFeedbackService;

        private const double ScanFailureVibrationDuration = 500;
        private const double ScanSuccessVibrationDuration = 250;

        public override ICommand BackCommand
        {
            get { return new Command(async () => await ExecuteOnceAsync(OnBackButtonPressed)); }
        }

        public ICommand OpenMenuCommand => new Command(async () =>
        {
            await ExecuteOnceAsync(async () =>
            {
                    await _navigationService.PushPage(new MenuPage(IoCContainer.Resolve<QRScannerMenuViewModel>()));
            });
        });

        private async Task OnBackButtonPressed()
        {
            DisableScanner();
            await _navigationService.PopPage();
        }

        public static ImagerScannerViewModel CreateImagerScannerViewModel()
        {
            return new ImagerScannerViewModel(
                IoCContainer.Resolve<IScannerFactory>(),
                IoCContainer.Resolve<ITokenProcessorService>(),
                IoCContainer.Resolve<ITextService>(),
                IoCContainer.Resolve<IPopupService>(),
                IoCContainer.Resolve<IDeviceFeedbackService>()
            );
        }

        public ImagerScannerViewModel(IScannerFactory scannerFactoryService,
            ITokenProcessorService tokenProcessorService,
            ITextService textService,
            IPopupService popupService,
            IDeviceFeedbackService deviceFeedbackService) : base()
        {
            _tokenProcessorService = tokenProcessorService;
            var translator = new DGCValueSetTranslator(textService);
            translator.SelectLanguage(LanguageSelection.English);
            _tokenProcessorService.SetDgcValueSetTranslator(translator);
            _scannerFactoryService = scannerFactoryService;
            _scanner = _scannerFactoryService.GetAvailableScanner();
            _popupService = popupService;
            _deviceFeedbackService = deviceFeedbackService;

            _scanner.SetSelectedScanner(new ScannerModel(id: "INTERNAL_IMAGER", name: "Imager", connectionState: true));
        }

        public void EnableScanner()
        {
            if (_scanner.IsEnabled == false)
            {
                _scanner.Enable();
                _scanner.Receiver.OnBarcodeScanned += BarcodeScanned;
                _scanner.SetConfig(_scannerFactoryService.GetScannerConfig());
            }
        }

        public void DisableScanner()
        {
            _scanner.Disable();
            _scanner.Receiver.OnBarcodeScanned -= BarcodeScanned;
        }

        private async void BarcodeScanned(object sender, StatusEventArgs e)
        {
            Debug.Print(e.Data);
            await HandleScanResult(e.Data);
        }

        public async Task HandleScanResult(String result)
        {
            if (_handlingResult) return;

            try
            {
                _handlingResult = true;

                TokenValidateResultModel model = await _tokenProcessorService.DecodePassportTokenToModel(result);

                if (model == null) return;

                if ((model.DecodedModel is Core.Services.Model.EuDCCModel._1._3._0.DCCPayload)
                        && model.ValidationResult == TokenValidateResult.Valid)
                {
                    await OnScanEUFinish(model.DecodedModel);
                    _deviceFeedbackService.Vibrate(ScanSuccessVibrationDuration);
                    _deviceFeedbackService.PlaySound(SoundKeys.VALID_SCAN_SOUND);
                }
                else if (model.ValidationResult == TokenValidateResult.Valid)
                {
                    await OnScanFinish(model.DecodedModel);
                    _deviceFeedbackService.Vibrate(ScanSuccessVibrationDuration);
                    _deviceFeedbackService.PlaySound(SoundKeys.VALID_SCAN_SOUND);
                }
                else
                {
                    await OnScanFailure(model);
                    _deviceFeedbackService.Vibrate(ScanFailureVibrationDuration);
                    _deviceFeedbackService.PlaySound(SoundKeys.INVALID_SCAN_SOUND);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _handlingResult = false;
            }
        }

        private async Task OnScanFinish(ITokenPayload model)
        {
            await _popupService.ShowScanSuccessPopup(model);
        }

        private async Task OnScanEUFinish(ITokenPayload model)
        {
            bool anyVaccinations = false;
            bool anyTestResults = false;
            bool anyRecovery = false;
            if (model is Core.Services.Model.EuDCCModel._1._3._0.DCCPayload cwtPayload)
            {
                anyVaccinations = cwtPayload.DCCPayloadData.DCC.Vaccinations?.Any() ?? false;
                anyTestResults = cwtPayload.DCCPayloadData.DCC.Tests?.Any() ?? false;
                anyRecovery = cwtPayload.DCCPayloadData.DCC.Recovery?.Any() ?? false;
            }

            if (PopupNavigation.Instance.PopupStack.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();

            if (anyVaccinations)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.PushPage(new ScanEuVaccineResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, model);
                });
            }
            else if (anyTestResults)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.PushPage(new ScanEuTestResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, model);
                });
            }
            else if (anyRecovery)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.PushPage(new ScanEuRecoveryResultView(), true, Enums.PageNavigationStyle.PushModallyFullscreen, model);
                });
            }
            else
            {
                await OnScanFailure(new TokenValidateResultModel() { ValidationResult = TokenValidateResult.Invalid });
            }
        }

        private async Task OnScanFailure(TokenValidateResultModel model)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await _navigationService.PushPage(new ScannerErrorPage(), data: model);
            });
        }
    }
}