using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Tests.TestMocks
{
    public class MockSmartHealthCardRepository : ISmartHealthCardRepository
    {
        public SmartHealthCardIssuer GetIssuerTrustResponse = new SmartHealthCardIssuer();

        public Task<SmartHealthCardVaccineInfo> GetVaccineInfo(SmartHealthCardCoding[] vaccineCodes)
        {
            return Task.FromResult(new SmartHealthCardVaccineInfo());
        }

        public Task<SmartHealthCardIssuer> GetIssuerTrust(string issuer)
        {
            return Task.FromResult(GetIssuerTrustResponse);
        }
    }
}
