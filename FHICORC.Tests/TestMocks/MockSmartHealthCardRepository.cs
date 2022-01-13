using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Tests.TestMocks
{
    public class MockSmartHealthCardRepository : ISmartHealthCardRepository
    {
        public MockSmartHealthCardRepository()
        {
        }

        public Task<SmartHealthCardVaccineInfo> GetVaccineInfo(SmartHealthCardCoding[] vaccineCodes)
        {
            return Task.FromResult(new SmartHealthCardVaccineInfo());
        }
    }
}
