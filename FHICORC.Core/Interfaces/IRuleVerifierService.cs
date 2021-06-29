using System.Collections.Generic;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;

namespace FHICORC.Core.Interfaces
{
    public interface IRuleVerifierService
    {
        List<RulesFeedbackData> Verify(ICollection<BusinessRule> rules, VerifyRulesModel data);
    }
}
