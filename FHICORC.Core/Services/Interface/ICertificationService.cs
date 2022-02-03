using System.Threading.Tasks;
using FHICORC.Core.Services.Model.CoseModel;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;

namespace FHICORC.Core.Services.Interface
{
    public interface ICertificationService
    {
        public Task VerifyCoseSign1Object(CoseSign1Object coseSign1Object);
        public Task VerifySHCSignature(JwsParts jws);
        public Task<SmartHealthCardIssuer> VerifySHCIssuer(JwsParts jws);
    }
}