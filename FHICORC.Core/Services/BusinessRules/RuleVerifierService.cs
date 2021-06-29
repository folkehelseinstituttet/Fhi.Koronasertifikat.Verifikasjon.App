using System;
using System.Collections.Generic;
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
        public RuleVerifierService()
        {
            _evaluator = new JsonLogicEvaluator(JsonLogicUtils.GetStringSupportedOperators());
        }

        private readonly JsonLogicEvaluator _evaluator;

        public List<RulesFeedbackData> Verify(ICollection<BusinessRule> rules, VerifyRulesModel verifyRulesModel)
        {
            var resultList = new List<RulesFeedbackData>();
            try
            {
                string json = JsonConvert.SerializeObject(verifyRulesModel);
                foreach (var rule in rules)
                {
                    try
                    {
                        var result = _evaluator.Apply(JObject.Parse(rule.Logic.ToString()), JObject.Parse(json));
                        bool successful;
                        bool.TryParse(result.ToString(), out successful);
                        if (successful)
                            resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier, RuleDescription = rule.Description, Result = Enum.RulesFeedbackResult.TRUE });
                        else
                            resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier, RuleDescription = rule.Description, Result = Enum.RulesFeedbackResult.FALSE });
                    }
                    catch (Exception)
                    {
                        resultList.Add(new RulesFeedbackData { RuleIdentifier = rule.Identifier,  RuleDescription = rule.Description, Result = Enum.RulesFeedbackResult.OPEN });
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
