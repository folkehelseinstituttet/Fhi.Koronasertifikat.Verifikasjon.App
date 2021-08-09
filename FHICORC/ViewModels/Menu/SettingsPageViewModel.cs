using FHICORC.Core.Data;
using FHICORC.Data;
using FHICORC.Enums;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using FHICORC.Services;
using System.Windows.Input;
using Xamarin.Forms;
using FHICORC.Models;
using FHICORC.Core.Services.Interface;
using System.Globalization;
using System;

namespace FHICORC.ViewModels.Menu
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ITextService _textService;
        private readonly IPreferencesService _preferencesService;
        private readonly ISecureStorageService<PublicKeyStorageModel> _secureStorageService;
        private readonly IPublicKeyService _publicKeyService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IBusinessRulesService _businessRulesService;
        private readonly IValueSetService _valueSetService;

        private bool isBokmalSelected;
        private bool isNynorskSelected;
        private bool isEnglishSelected;
        private string isBokmalSelectedText;
        private string isNynorskSelectedText;
        private string isEnglishSelectedText;
        private bool isChangeLanguageSelected = false;

        public bool IsBokmalSelected
        {
            get { return isBokmalSelected; }
            set
            {
                isBokmalSelected = value;
                if (isBokmalSelected)
                    DisplayAppRestartDialog(LanguageSelection.Bokmal);
                OnPropertyChanged(nameof(IsBokmalSelected));
            }
        }
        public bool IsNynorskSelected
        {
            get { return isNynorskSelected; }
            set
            {
                isNynorskSelected = value;
                if (isNynorskSelected)
                    DisplayAppRestartDialog(LanguageSelection.Nynorsk);
                OnPropertyChanged(nameof(IsNynorskSelected));
            }
        }
        public bool IsEnglishSelected
        {
            get { return isEnglishSelected; }
            set
            {
                isEnglishSelected = value;
                if (isEnglishSelected)
                    DisplayAppRestartDialog(LanguageSelection.English);
                OnPropertyChanged(nameof(IsEnglishSelected));
            }
        }

        public bool IsChangeLanguageSelected
        {
            get { return isChangeLanguageSelected; }
            set
            {
                isChangeLanguageSelected = value;
                OnPropertyChanged(nameof(IsChangeLanguageSelected));
            }
        }

        public string IsBokmalSelectedText
        {
            get { return isBokmalSelectedText; }
            set
            {
                isBokmalSelectedText = value;
                OnPropertyChanged(nameof(IsBokmalSelectedText));
            }
        }
        public string IsNynorskSelectedText
        {
            get { return isNynorskSelectedText; }
            set
            {
                isNynorskSelectedText = value;
                OnPropertyChanged(nameof(IsNynorskSelectedText));
            }
        }
        public string IsEnglishSelectedText
        {
            get { return isEnglishSelectedText; }
            set
            {
                isEnglishSelectedText = value;
                OnPropertyChanged(nameof(IsEnglishSelectedText));
            }
        }

        private string _sectionTitleSecurity = "SETTINGS_SECTION_2_TITLE".Translate();

        public string SectionTitleSecurity
        {
            get
            {
                return _sectionTitleSecurity;
            }
            set
            {
                _sectionTitleSecurity = value;
                OnPropertyChanged(nameof(SectionTitleSecurity));
            }
        }

        private string _sectionTitleScanner = "QRSCANNER_SETTINGS_SECTION_TITLE".Translate();

        public string SectionTitleScanner
        {
            get
            {
                return _sectionTitleScanner;
            }
            set
            {
                _sectionTitleScanner = value;
                OnPropertyChanged((nameof(SectionTitleScanner)));
            }
        }

        public virtual bool FromLandingPage => false;

        private string _settingSound = "SETTINGS_SOUND".Translate();
        public string SettingsSound
        {
            get
            {
                return _settingSound;
            }
            set
            {
                _settingSound = value;
                OnPropertyChanged(nameof(SettingsSound));
            }
        }

        private string _settingsVibration = "SETTINGS_VIBRATION".Translate();
        public string SettingsVibration
        {
            get
            {
                return _settingsVibration;
            }
            set
            {
                _settingsVibration = value;
                OnPropertyChanged(nameof(SettingsVibration));
            }
        }

        public string LastUpdated =>
            string.Format("SETTINGS_LAST_UPDATED".Translate(),
                _secureStorageService.GetSecureStorageAsync(
                        SecureStorageKeys.PUBLIC_KEY)
                            .GetAwaiter()
                            .GetResult()
                            .LastFetchTimestamp
                            .ToString(
                                "f",
                                CultureInfo.CreateSpecificCulture(LocaleService.Current.GetLanguage().ToISOCode()
                            )
                )
            );

        private DateTime? lastClicked;
        private int maxClickIntervalInMinutes = 1;

        public ICommand FetchKeysCommand => new Command(async () =>
        {
            // Limit to 1 request per minute
            if (lastClicked?.AddMinutes(maxClickIntervalInMinutes) > _dateTimeService.Now)
            {
                return;
            }
            await _publicKeyService.FetchPublicKeyFromBackend(false);
            await _businessRulesService.FetchBusinessRulesFromBackend(false);
            await _textService.LoadRemoteLocales();
            long lastTimeFetchedValuesets = _preferencesService.GetUserPreferenceAsLong(PreferencesKeys.LAST_TIME_FETCHED_VALUESETS);
            await _valueSetService.FetchAndSaveLatestVersionOfValueSets(lastTimeFetchedValuesets);
            RaisePropertyChanged(() => LastUpdated);
            lastClicked = _dateTimeService.Now;
        });


        public bool VibrationSettingEnabled =>
            _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.SCANNER_VIBRATION_SETTING);

        public bool SoundSettingEnabled =>
            _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.SCANNER_SOUND_SETTING);

        public SettingsPageViewModel(
            IDialogService dialogService,
            ITextService textService,
            IPreferencesService preferencesService,
            ISecureStorageService<PublicKeyStorageModel> secureStorageService,
            IPublicKeyService publicKeyService,
            IDateTimeService dateTimeService,
            IBusinessRulesService businessRulesService,
            IValueSetService valueSetService)
        {
            _dialogService = dialogService;
            _textService = textService;
            _preferencesService = preferencesService;
            _secureStorageService = secureStorageService;
            _publicKeyService = publicKeyService;
            _dateTimeService = dateTimeService;
            _businessRulesService = businessRulesService;
            _valueSetService = valueSetService;

            SetRadioButtons();
            setAccessibilityTextOnRadioButtons();
        }

        private void SetRadioButtons()
        {
            LanguageSelection languageSelection = LocaleService.Current.GetLanguage();
            IsBokmalSelected = languageSelection == LanguageSelection.Bokmal;
            IsNynorskSelected = languageSelection == LanguageSelection.Nynorsk;
            IsEnglishSelected = languageSelection == LanguageSelection.English;
        }

        private void setAccessibilityTextOnRadioButtons()
        {
            IsBokmalSelectedText = IsBokmalSelected ? "SETTINGS_RADIO_BUTTON_CHOSEN".Translate() : "SETTINGS_RADIO_BUTTON_NOT_CHOSEN".Translate();
            IsNynorskSelectedText = IsNynorskSelected ? "SETTINGS_RADIO_BUTTON_CHOSEN".Translate() : "SETTINGS_RADIO_BUTTON_NOT_CHOSEN".Translate();
            IsEnglishSelectedText = IsEnglishSelected ? "SETTINGS_RADIO_BUTTON_CHOSEN".Translate() : "SETTINGS_RADIO_BUTTON_NOT_CHOSEN".Translate();
        }


        private async void DisplayAppRestartDialog(LanguageSelection newLanguage)
        {
            LanguageSelection currentLanguage = LocaleService.Current.GetLanguage();
            if (newLanguage == currentLanguage)
                return;

            IsChangeLanguageSelected = true;
            string title = $"{"SETTINGS_CHOOSE_LANGUAGE_DIALOG_TITLE_NB".Translate()}/\n"
                + "SETTINGS_CHOOSE_LANGUAGE_DIALOG_TITLE_EN".Translate();
            string content = $"{"SETTINGS_CHOOSE_LANGUAGE_DIALOG_CONTENT_NB".Translate()}/\n"
                + "SETTINGS_CHOOSE_LANGUAGE_DIALOG_CONTENT_EN".Translate();
            string accept = "DIALOG_OK_BUTTON".Translate();
            string cancel = "DIALOG_CANCEL_BUTTON".Translate();
            bool result = await _dialogService.ShowAlertAsync(title, content, accept, cancel);
            if (result)
            {
                if (IsEnglishSelected)
                {
                    _textService.SetLocale("en");
                }
                if (IsNynorskSelected)
                {
                    _textService.SetLocale("nn");
                }
                if (IsBokmalSelected)
                {
                    _textService.SetLocale("nb");
                }

                if (this.FromLandingPage)
                {
                    _navigationService.OpenLandingPage();
                }
            }
            else
            {
                SetRadioButtons();
                setAccessibilityTextOnRadioButtons();
            }
            IsChangeLanguageSelected = false;
        }

        public override ICommand BackCommand => new Command(async () =>
        {
            await ExecuteOnceAsync(async () =>
            {
                if (IsChangeLanguageSelected)
                {
                    return;
                }
                else
                {
                    await _navigationService.PopPage();
                }
            });
        });

        public void VibrationSettingChanged(bool isToggled)
        {
            _preferencesService.SetUserPreference(PreferencesKeys.SCANNER_VIBRATION_SETTING, isToggled);
        }

        public void SoundSettingChanged(bool isToggled)
        {
            _preferencesService.SetUserPreference(PreferencesKeys.SCANNER_SOUND_SETTING, isToggled);
        }
    }
}