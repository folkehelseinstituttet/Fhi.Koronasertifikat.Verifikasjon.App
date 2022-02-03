using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.CoseModel;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;

namespace FHICORC.Tests.TestMocks
{
    public class MockCertificationService : ICertificationService
    {
        public Task VerifyCoseSign1Object(CoseSign1Object coseSign1Object)
        {
            return Task.FromResult(true);
        }

        public Task<SmartHealthCardIssuer> VerifySHCIssuer(JwsParts jws)
        {
            return Task.FromResult(new SmartHealthCardIssuer());
        }

        public Task VerifySHCSignature(JwsParts jws)
        {
            return Task.FromResult(true);
        }
    }
}
