using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Data;
using FHICORC.Enums;
using FHICORC.Models;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Certificates;
using FHICORC.Views.ScannerPages;
using Xamarin.Forms;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScanEuTestResultViewModel : InfoTestTextViewModel
    {
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string _fullNameAccessibilityText;
        public string FullNameAccessibilityText
        {
            get => _fullNameAccessibilityText;
            set
            {
                _fullNameAccessibilityText = value;
                OnPropertyChanged(nameof(FullNameAccessibilityText));
            }
        }

        private string _dateOfBirth;
        public string DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        private string _dateOfBirthAccessibilityText;
        public string DateOfBirthAccessibilityText
        {
            get => _dateOfBirthAccessibilityText;
            set
            {
                _dateOfBirthAccessibilityText = value;
                OnPropertyChanged(nameof(DateOfBirthAccessibilityText));
            }
        }

        private Color _ruleBackgroundColor;
        public Color RuleBackgroundColor
        {
            get => _ruleBackgroundColor;
            set
            {
                _ruleBackgroundColor = value;
                OnPropertyChanged(nameof(RuleBackgroundColor));
            }
        }

        public int RulesEnginePassed { get; set; }
        public int RulesEngineResultCount { get; set; }

        private string _numberOfRulesFulfilled;
        public string NumberOfRulesFulfilled
        {
            get => _numberOfRulesFulfilled;
            set
            {
                _numberOfRulesFulfilled = value;
                OnPropertyChanged(nameof(NumberOfRulesFulfilled));
            }
        }

        private string _numberOfRulesFulfilledAccessibilityText;
        public string NumberOfRulesFulfilledAccessibilityText
        {
            get => _numberOfRulesFulfilledAccessibilityText;
            set
            {
                _numberOfRulesFulfilledAccessibilityText = value;
                OnPropertyChanged(nameof(NumberOfRulesFulfilledAccessibilityText));
            }
        }

        private string _bannerText;
        public string BannerText
        {
            get => _bannerText;
            set
            {
                _bannerText = value;
                OnPropertyChanged(nameof(BannerText));
            }
        }

        private Color _bannerColor;
        public Color BannerColor
        {
            get => _bannerColor;
            set
            {
                _bannerColor = value;
                OnPropertyChanged(nameof(BannerColor));
            }
        }

        private Color _bannerTextColor;
        public Color BannerTextColor
        {
            get => _bannerTextColor;
            set
            {
                _bannerTextColor = value;
                OnPropertyChanged(nameof(BannerTextColor));
            }
        }

        private string _rulesFulfilledText;
        public string RulesFulfilledText
        {
            get => _rulesFulfilledText;
            set
            {
                _rulesFulfilledText = value;
                OnPropertyChanged(nameof(RulesFulfilledText));
            }
        }

        private string _rulesAccessibilityText;
        public string RulesAccessibilityText
        {
            get => _rulesAccessibilityText;
            set
            {
                _rulesFulfilledText = value;
                OnPropertyChanged(nameof(RulesAccessibilityText));
            }
        }

        public bool IsBorderControlOn => _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.BORDER_CONTROL_ON);

        public RulesFeedbackViewModel RulesFeedbackViewModel { get; set; }

        private readonly IPreferencesService _preferencesService;

        public string RepeatedText => string.Concat(Enumerable.Repeat($"{BannerText}         ", 10));


        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ICommand ShowRulesInfoCommand => new Command(async () => await ExecuteOnceAsync(ShowRulesInfo));

        public ScanEuTestResultViewModel(ITimer timer,
            IPreferencesService preferencesService,
            IDigitalGreenValueSetTranslatorFactory digitalGreenValueSetTranslatorFactory) : base(timer, digitalGreenValueSetTranslatorFactory)
        {
            ShowTextInEnglish = true;
            ShowHeader = true;
            _preferencesService = preferencesService;
            CheckControlType();
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                if (navigationData is TokenValidateResultModel tokenValidateResultModel)
                {
                    if (tokenValidateResultModel.DecodedModel is Core.Services.Model.EuDCCModel._1._3._0.DCCPayload cwt)
                    {
                        FullName = cwt.DCCPayloadData.DCC.PersonName.FullName;
                        FullNameAccessibilityText = cwt.DCCPayloadData.DCC.PersonName.FullName.ToLower();
                        DateOfBirth = cwt.DCCPayloadData.DCC.DateOfBirth;
                        PassportViewModel.PassportData = new PassportData(string.Empty, cwt);
                        RulesFeedbackViewModel = new RulesFeedbackViewModel(tokenValidateResultModel.RulesFeedBacks);
                        RulesEnginePassed = RulesFeedbackViewModel.RulesEngineResult.Where(x => x.Result == RulesFeedbackResult.TRUE).Count();
                        RulesEngineResultCount = RulesFeedbackViewModel.RulesEngineResult.Count;
                        NumberOfRulesFulfilled = string.Format("RULES_ENGINE_FULFILLED_COUNT".Translate(), RulesEnginePassed, RulesEngineResultCount);
                        NumberOfRulesFulfilledAccessibilityText = string.Format("RULES_ENGINE_FULFILLED_COUNT_ACCESSIBILITY_TEXT".Translate(), RulesEnginePassed, RulesEngineResultCount);
                        UpdateRuleColorAndText();
                        UpdateView();
                        SetAccessibilityTextDateOfBirth();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to initialise TestResult scan: {e}");
            }

            return base.InitializeAsync(navigationData);
        }

        public void SetAccessibilityTextDateOfBirth()
        {
            try
            {
                var dateOfBirth = DateTime.ParseExact(DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateOfBirthAccessibilityText = string.Format("{0:dd. MMMM yyyy}", dateOfBirth);
            }
            catch (FormatException)
            {
                DateOfBirthAccessibilityText = DateOfBirth;
            }
        }

        private async Task ShowRulesInfo()
        {
            await _navigationService.PushPage(new RulesInfoModalView(RulesFeedbackViewModel, EuPassportType.TEST), true, PageNavigationStyle.PushModallySheetPageIOS);
        }

        private void CheckControlType()
        {
            if (_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.BORDER_CONTROL_ON))
            {
                BannerColor = Color.FromHex("#32345C");
                BannerTextColor = Color.FromHex("#F3F9FB");
                BannerText = "SCANNER_EU_BANNER_TEXT".Translate();
            }
            else
            {
                BannerColor = Color.FromHex("F3F9FB");
                BannerTextColor = Color.FromHex("#32345C");
                BannerText = "SCANNER_NO_BANNER_TEXT".Translate();
            }
        }

        private void UpdateRuleColorAndText()
        {
            if (RulesEnginePassed == RulesEngineResultCount)
            {
                RuleBackgroundColor = Color.FromHex("#D9F0D4");
                if (IsBorderControlOn)
                {
                    RulesFulfilledText = NumberOfRulesFulfilled;
                    RulesAccessibilityText = NumberOfRulesFulfilledAccessibilityText;
                }
                else
                {
                    RulesFulfilledText = "RULES_ENGINE_DOMESTIC_FULFILLED".Translate();
                    RulesAccessibilityText = "RULES_ENGINE_DOMESTIC_FULFILLED".Translate();
                }
            }
            else
            {
                RuleBackgroundColor = Color.FromHex("#FBB5AD");
                if (IsBorderControlOn)
                {
                    RulesFulfilledText = NumberOfRulesFulfilled;
                    RulesAccessibilityText = NumberOfRulesFulfilledAccessibilityText;
                }
                else
                {
                    RulesFulfilledText = "RULES_ENGINE_DOMESTIC_NOT_FULFILLED".Translate();
                    RulesAccessibilityText = "RULES_ENGINE_DOMESTIC_NOT_FULFILLED".Translate();
                }
            }
        }
    }
}