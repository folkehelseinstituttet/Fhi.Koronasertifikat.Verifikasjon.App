using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Core.Services.Model
{
    public class TokenValidateResultModel
    {
        public TokenValidateResult ValidationResult { get; set; } = TokenValidateResult.Invalid;
        public ITokenPayload DecodedModel { get; set; }
    }
}