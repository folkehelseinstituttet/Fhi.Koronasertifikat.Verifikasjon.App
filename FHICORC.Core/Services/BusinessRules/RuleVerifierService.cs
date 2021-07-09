using System;
using System.Collections.Generic;
using System.Linq;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Utils;
using JsonLogic.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FHICORC.Core.Services
{
    public class RuleVerifierService : IRuleVerifierService
    {
        private readonly IPreferencesService _preferencesService;
        public RuleVerifierService(IPreferencesService preferencesService)
        {
            _evaluator = new JsonLogicEvaluator(JsonLogicUtils.GetStringSupportedOperators());
            _preferencesService = preferencesService;
        }

        private readonly JsonLogicEvaluator _evaluator;

        public List<RulesFeedbackData> Verify(ICollection<BusinessRule> rules, VerifyRulesModel verifyRulesModel)
        {
            var currentLanguage = _preferencesService.GetUserPreferenceAsString("LANGUAGE_SETTING");
            var resultList = new List<RulesFeedbackData>();
            try
            {
                string json = JsonConvert.SerializeObject(verifyRulesModel);
                foreach (var rule in rules)
                {
                    var description = rule.Description.Where(x => x.LanguageCode == currentLanguage).FirstOrDefault()?.Description;
                    try
                    {
                        var result = _evaluator.Apply(JObject.Parse(rule.Logic.ToString()), JObject.Parse(json));
                        bool successful;
                        bool.TryParse(result.ToString(), out successful);
                        if (successful)
                            resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier, RuleDescription = description, Result = Enum.RulesFeedbackResult.TRUE });
                        else
                            resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier, RuleDescription = description, Result = Enum.RulesFeedbackResult.FALSE });
                    }
                    catch (Exception)
                    {
                        resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier,  RuleDescription = description, Result = Enum.RulesFeedbackResult.OPEN });
                    }
                }
            }
            catch (Exception)
            {
            }

            return resultList;
        }
    }
}
