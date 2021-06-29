using System.Collections.Generic;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.BusinessRules;

namespace FHICORC.Core.Services.Model
{
    public class TokenValidateResultModel
    {
        public TokenValidateResult ValidationResult { get; set; } = TokenValidateResult.Invalid;
        public ITokenPayload DecodedModel { get; set; }
        public List<RulesFeedbackData> RulesFeedBacks { get; set; }
    }
}