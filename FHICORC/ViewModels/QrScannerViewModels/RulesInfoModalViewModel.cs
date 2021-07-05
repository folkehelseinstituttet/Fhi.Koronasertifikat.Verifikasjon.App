using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.ViewModels.Base;
using FHICORC.ViewModels.Certificates;
using Xamarin.CommunityToolkit.ObjectModel;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class RulesInfoModalViewModel : ContentSheetPageNoBackButtonOnIOSViewModel
    {
        public RulesFeedbackViewModel RulesFeedbackViewModel { get; set; }
        private EuPassportType _euPassportType { get; set; }

        private ObservableCollection<RulesFeedbackData> _rulesFeedback;
        public ObservableCollection<RulesFeedbackData> RulesFeedback
        {
            get
            {
                return _rulesFeedback;
            }
            set
            {
                _rulesFeedback = value;
            }
        }

        public string RuleIdentifier { get; set; }
        public string RuleDescription { get; set; }
        public string RuleTypeHeader { get; set; }
        public string RuleSubHeader { get; set; }

        public bool IsTestSelected => _euPassportType == EuPassportType.TEST;
        public bool IsVaccineSelected => _euPassportType == EuPassportType.VACCINE;
        public bool IsRecoverySelected => _euPassportType == EuPassportType.RECOVERY;

        public int RulesEnginePassed { get; set; }
        public int RulesEngineFailed { get; set; }
        public int RulesEngineOpen { get; set; }
        public int RulesEngineResultCount { get; set; }

        private string _numberOfRulesPassed;
        public string NumberOfRulesPassed
        {
            get => _numberOfRulesPassed;
            set
            {
                _numberOfRulesPassed = value;
                OnPropertyChanged(nameof(NumberOfRulesPassed));
            }
        }

        private string _numberOfRulesFailed;
        public string NumberOfRulesFailed
        {
            get => _numberOfRulesFailed;
            set
            {
                _numberOfRulesFailed = value;
                OnPropertyChanged(nameof(NumberOfRulesFailed));
            }
        }

        private string _numberOfRulesOpen;
        public string NumberOfRulesOpen
        {
            get => _numberOfRulesOpen;
            set
            {
                _numberOfRulesOpen = value;
                OnPropertyChanged(nameof(NumberOfRulesOpen));
            }
        }

        private string _numberOfRulesPassedAccessibilityText;
        public string NumberOfRulesPassedAccessibilityText
        {
            get => _numberOfRulesPassedAccessibilityText;
            set
            {
                _numberOfRulesPassedAccessibilityText = value;
                OnPropertyChanged(nameof(NumberOfRulesPassedAccessibilityText));
            }
        }


        private string _numberOfRulesFailedAccessibilityText;
        public string NumberOfRulesFailedAccessibilityText
        {
            get => _numberOfRulesFailedAccessibilityText;
            set
            {
                _numberOfRulesFailedAccessibilityText = value;
                OnPropertyChanged(nameof(NumberOfRulesFailedAccessibilityText));
            }
        }

        private string _numberOfRulesOpenAccessibilityText;
        public string NumberOfRulesOpenAccessibilityText
        {
            get => _numberOfRulesOpenAccessibilityText;
            set
            {
                _numberOfRulesOpenAccessibilityText = value;
                OnPropertyChanged(nameof(NumberOfRulesOpenAccessibilityText));
            }
        }



        public RulesInfoModalViewModel(RulesFeedbackViewModel rulesFeedbackViewModel, EuPassportType euPassportType)
        {
            RulesFeedbackViewModel = rulesFeedbackViewModel;
            _euPassportType = euPassportType;
            RulesFeedback = new ObservableCollection<RulesFeedbackData>();
            UpdateText();
        }

        public void UpdateView()
        {
            OnPropertyChanged(nameof(IsTestSelected));
            OnPropertyChanged(nameof(IsRecoverySelected));
            OnPropertyChanged(nameof(IsVaccineSelected));
        }

        public void UpdateText()
        {
            foreach (RulesFeedbackData rulesFeedbackData in RulesFeedbackViewModel.RulesEngineResult)
            {
                RuleIdentifier = rulesFeedbackData.RuleIdentifier;
                RuleDescription = rulesFeedbackData.RuleDescription;
                RulesFeedback.Add(rulesFeedbackData);
            }
            RulesFeedback = new ObservableCollection<RulesFeedbackData>(RulesFeedback.OrderBy(x => x.Result));
            var firstTrue = RulesFeedback.FirstOrDefault(x => x.Result == RulesFeedbackResult.TRUE);
            if (firstTrue != null)
                firstTrue.IsFirstWithResult = true;
            var firstOpen = RulesFeedback.FirstOrDefault(x => x.Result == RulesFeedbackResult.OPEN);
            if (firstOpen != null)
                firstOpen.IsFirstWithResult = true;
            var firstFalse = RulesFeedback.FirstOrDefault(x => x.Result == RulesFeedbackResult.FALSE);
            if (firstFalse != null)
                firstFalse.IsFirstWithResult = true;

            if (IsRecoverySelected)
            {
                RuleTypeHeader = "RULES_ENGINE_VERIFY_HEADER_RECOVERY".Translate();
            }

            if (IsVaccineSelected)
            {
                RuleTypeHeader = "RULES_ENGINE_VERIFY_HEADER_VACCINE".Translate();
            }

            if (IsTestSelected)
            {
                RuleTypeHeader = "RULES_ENGINE_VERIFY_HEADER_TEST".Translate();
            }

            RulesEnginePassed = RulesFeedbackViewModel.RulesEngineResult.Where(x => x.Result == RulesFeedbackResult.TRUE).Count();
            RulesEngineFailed = RulesFeedbackViewModel.RulesEngineResult.Where(x => x.Result == RulesFeedbackResult.FALSE).Count();
            RulesEngineOpen = RulesFeedbackViewModel.RulesEngineResult.Where(x => x.Result == RulesFeedbackResult.OPEN).Count();
            RulesEngineResultCount = RulesFeedbackViewModel.RulesEngineResult.Count;

            NumberOfRulesPassed = string.Format("RULES_ENGINE_VERIFY_PASSED".Translate(), RulesEnginePassed, RulesEngineResultCount);
            NumberOfRulesFailed = string.Format("RULES_ENGINE_VERIFY_FAILED".Translate(), RulesEngineFailed, RulesEngineResultCount);
            NumberOfRulesOpen = string.Format("RULES_ENGINE_FULFILLED_COUNT_ACCESSIBILITY_TEXT".Translate(), RulesEngineOpen, RulesEngineResultCount);

            NumberOfRulesPassedAccessibilityText = string.Format("RULES_ENGINE_VERIFY_PASSED_ACCESSIBILITY_TEXT".Translate(), RulesEnginePassed, RulesEngineResultCount);
            NumberOfRulesFailedAccessibilityText = string.Format("RULES_ENGINE_VERIFY_FAILED_ACCESSIBILITY_TEXT".Translate(), RulesEngineFailed, RulesEngineResultCount);
            NumberOfRulesOpenAccessibilityText = string.Format("RULES_ENGINE_VERIFY_OPEN_ACCESSIBILITY_TEXT".Translate(), RulesEngineOpen, RulesEngineResultCount);

        }
    }
}
