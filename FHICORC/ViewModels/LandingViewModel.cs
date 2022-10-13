using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.ViewModels.Base;
using FHICORC.Views;
using Xamarin.Forms;
using FHICORC.Services;
using FHICORC.Enums;
using FHICORC.Services.Interfaces;
using FHICORC.Views.ScannerPages;
using Xamarin.CommunityToolkit.ObjectModel;
using FHICORC.Core.Data;
using FHICORC.Data;
using System;

namespace FHICORC.ViewModels
{
    public class LandingViewModel : BaseViewModel
    {
        public string RetrieveCertificateText => "LANDING_PAGE_COVID_STATUS".Translate();
        public string OpenScannerText => "LANDING_PAGE_QR_CODE".Translate();
        public string LandingPageTitle => "LANDING_PAGE_TITLE".Translate();
        public string LanguageChangeButtonText => "LANDING_PAGE_LANGUAGE_CHANGE_BUTTON_TEXT".Translate();
        public string HelpButton => "HELP".Translate();
        public string SelectControlType => "LANDING_PAGE_CONTROL_TYPE".Translate();
        public string ScannerEURadioBtnText => "SCANNER_EU_BANNER_TEXT".Translate();
        public string ScannerNORadioBtnText => "SCANNER_NO_BANNER_TEXT".Translate();

        public ICommand ChangeLanguageCommand => new Command(async () => await ExecuteOnceAsync(DisplayChangeLanguageDialog));
        public ICommand OpenScannerCommand => new Command(async () => await ExecuteOnceAsync(GoToScannerPage));

        private readonly IDialogService _dialogService;
        private readonly IPreferencesService _preferencesService;
        private IScannerFactory _scannerFactoryService;

        private bool _borderControlOn;
        public bool BorderControlOn
        {
            get => _borderControlOn;
            set
            {
                if (_borderControlOn == value)
                    return;
                _borderControlOn = value;
                DomesticControlOn = !BorderControlOn;
                _preferencesService.SetUserPreference(PreferencesKeys.BORDER_CONTROL_ON, value);
                UpdateScannerButton();
                OnPropertyChanged(nameof(BorderControlOn));
                setAccessibilityTextOnRadioButtons();
            }
        }

        private bool _domesticControlOn;
        public bool DomesticControlOn
        {
            get => _domesticControlOn;
            set
            {
                if (_domesticControlOn == value)
                    return;
                _domesticControlOn = value;
                BorderControlOn = !DomesticControlOn;
                _preferencesService.SetUserPreference(PreferencesKeys.DOMESTIC_CONTROL_ON, value);
                UpdateScannerButton();
                OnPropertyChanged(nameof(DomesticControlOn));
                setAccessibilityTextOnRadioButtons();
            }
        }

        private string _radioButtonBorderText;
        public string RadioButtonBorderText
        {
            get => _radioButtonBorderText;
            set
            {
                _radioButtonBorderText = value;
                OnPropertyChanged(nameof(RadioButtonBorderText));
            }
        }


        private string _radioButtonDomesticText;
        public string RadioButtonDomesticText
        {
            get => _radioButtonDomesticText;
            set
            {
                _radioButtonDomesticText = value;
                OnPropertyChanged(nameof(RadioButtonDomesticText));
            }
        }


        private bool _isScannerButtonEnabled;
        public bool IsScannerButtonEnabled
        {
            get => _isScannerButtonEnabled;
            set
            {
                _isScannerButtonEnabled = value;
                OnPropertyChanged(nameof(IsScannerButtonEnabled));
            }
        }

        public LandingViewModel(IDialogService dialogService, IScannerFactory scannerFactoryService, IPreferencesService preferencesService)
        {
            _dialogService = dialogService;
            _scannerFactoryService = scannerFactoryService;
            _preferencesService = preferencesService;
            BorderControlOn = _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.BORDER_CONTROL_ON);
            DomesticControlOn = _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.DOMESTIC_CONTROL_ON);
            setAccessibilityTextOnRadioButtons();
        }


        public void setAccessibilityTextOnRadioButtons()
        {
            RadioButtonBorderText = BorderControlOn ? "SETTINGS_RADIO_BUTTON_CHOSEN".Translate() : "SETTINGS_RADIO_BUTTON_NOT_CHOSEN".Translate();
            RadioButtonDomesticText = DomesticControlOn ? "SETTINGS_RADIO_BUTTON_CHOSEN".Translate() : "SETTINGS_RADIO_BUTTON_NOT_CHOSEN".Translate();
        }

        private async Task GoToScannerPage()
        {
            if (_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.TERMS_ACCEPTED))
            {
                //if (!App.task.IsCompleted)
                //{
                await _navigationService.PushPage(new NativeLoadingPage());
                //await App.task;
                await _navigationService.PopPage();
                //}

                if (_scannerFactoryService.GetAvailableScanner() == null)
                {
                    await _navigationService.PushPage(new QRScannerPage());
                }
                else
                {
                    await _navigationService.PushPage(new ImagerScannerPage());
                }
            }
            else
            {
                await _navigationService.OpenAcceptTermsPage();
            }
        }

        private async Task DisplayChangeLanguageDialog()
        {
            string title = $"{"SETTINGS_CHOOSE_LANGUAGE_DIALOG_TITLE_NB".Translate()}/\n"
                + "SETTINGS_CHOOSE_LANGUAGE_DIALOG_TITLE_EN".Translate();
            string content = $"{"SETTINGS_CHOOSE_LANGUAGE_DIALOG_CONTENT_DANISH".Translate()}\n"
                + "SETTINGS_CHOOSE_LANGUAGE_DIALOG_CONTENT_ENGLISH".Translate();
            string accept = "CHANGE_LANGUAGE_DIALOG_OK_BUTTON".Translate();
            bool result = await _dialogService.ShowLanguageSelectionDialog(title, content, accept);
            if (result)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            OnPropertyChanged(nameof(RetrieveCertificateText));
            OnPropertyChanged(nameof(OpenScannerText));
            OnPropertyChanged(nameof(LandingPageTitle));
            OnPropertyChanged(nameof(LanguageChangeButtonText));
            OnPropertyChanged(nameof(SelectControlType));
            OnPropertyChanged(nameof(ScannerEURadioBtnText));
            OnPropertyChanged(nameof(ScannerNORadioBtnText));
            setAccessibilityTextOnRadioButtons();
        }

        private void UpdateScannerButton()
        {
            if (!BorderControlOn && !DomesticControlOn)
                IsScannerButtonEnabled = false;
            else
                IsScannerButtonEnabled = true;
        }
    }
}
