using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FHICORC.ViewModels.Certificates
{
    public class RulesFeedbackViewModel
    {
        private bool DoPass;
        public string RulesEngineResultTitle => DoPass? "RULES_ENGINE_PASS_TITLE".Translate() : "RULES_ENGINE_FAIL_TITLE".Translate();

        public string RuleIdentifierTableHeader => "RULES_ENGINE_RULE_IDENTIFIER_TABLE_HEADER".Translate();

        public string RuleDescriptionTableHeader => "RULES_ENGINE_RULE_DESCRIPTION_TABLE_HEADER".Translate();

        public string ResultTableHeader => "RULES_ENGINE_RESULT_TABLE_HEADER".Translate();

        public string CurrentTableHeader => "RULES_ENGINE_CURRENT_TABLE_HEADER".Translate();

        public List<string> RulesFeedbackList { get; set; }

        public List<RulesFeedbackData> RulesEngineResult { get; set; }

        public RulesFeedbackViewModel(List<RulesFeedbackData> rulesEngineResult)
        {
            RulesEngineResult = rulesEngineResult;
            DoPass = !RulesEngineResult.Any();
            RulesFeedbackList = RulesEngineResult.Select(feedback => feedback.RuleDescription).ToList();
        }

        public RulesFeedbackViewModel(List<string> rulesFeedbackList)
        {
            RulesFeedbackList = rulesFeedbackList;
            DoPass = !RulesFeedbackList.Any();
            RulesEngineResult = RulesFeedbackList.Select(feedback =>
                        new RulesFeedbackData
                        {
                            RuleDescription = feedback,
                            Result = RulesFeedbackResult.FALSE
                        }).ToList();
        }
    }
}
