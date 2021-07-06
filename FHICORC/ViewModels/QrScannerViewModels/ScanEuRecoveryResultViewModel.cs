using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Enum;
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
    public class ScanEuRecoveryResultViewModel : InfoRecoveryTextViewModel
    {
        private string _fullName;
        private string _dateOfBirth;

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
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

        public RulesFeedbackViewModel RulesFeedbackViewModel { get; set; }

        private readonly IPreferencesService _preferencesService;

        public string RepeatedText => string.Concat(Enumerable.Repeat($"{BannerText}         ", 10));

        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ICommand ShowRulesInfoCommand => new Command(async () => await ExecuteOnceAsync(ShowRulesInfo));

        public ScanEuRecoveryResultViewModel(ITimer timer, IPreferencesService preferencesService) : base(timer)
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
                        DateOfBirth = cwt.DCCPayloadData.DCC.DateOfBirth;
                        PassportViewModel.PassportData = new PassportData(string.Empty, cwt);
                        RulesFeedbackViewModel = new RulesFeedbackViewModel(tokenValidateResultModel.RulesFeedBacks);
                        RulesEnginePassed = RulesFeedbackViewModel.RulesEngineResult.Where(x => x.Result == RulesFeedbackResult.TRUE).Count();
                        RulesEngineResultCount = RulesFeedbackViewModel.RulesEngineResult.Count;
                        NumberOfRulesFulfilled = string.Format("RULES_ENGINE_FULFILLED_COUNT".Translate(), RulesEnginePassed, RulesEngineResultCount);
                        NumberOfRulesFulfilledAccessibilityText = string.Format("RULES_ENGINE_FULFILLED_COUNT_ACCESSIBILITY_TEXT".Translate(), RulesEnginePassed, RulesEngineResultCount);
                        if (RulesEnginePassed == RulesEngineResultCount)
                        {
                            RuleBackgroundColor = Color.FromHex("#D9F0D4");
                        }
                        else
                        {
                            RuleBackgroundColor = Color.FromHex("#FBB5AD");
                        }
                        UpdateView();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to initialise TestResult scan: {e}");
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task ShowRulesInfo()
        {
            await _navigationService.PushPage(new RulesInfoModalView(RulesFeedbackViewModel, EuPassportType.RECOVERY), true, PageNavigationStyle.PushModallySheetPageIOS);
        }

        private void CheckControlType()
        {
            if (_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.BORDER_CONTROL_ON))
            {
                BannerColor = Color.FromHex("#32345C");
                BannerText = "SCANNER_EU_BANNER_TEXT".Translate();
            }
            else
            {
                BannerColor = Color.FromHex("#B22A5A");
                BannerText = "SCANNER_NO_BANNER_TEXT".Translate();
            }
        }
    }
}