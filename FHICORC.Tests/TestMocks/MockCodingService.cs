using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Tests.TestMocks
{
    public class MockCodingService : ICodingService
    {
        public Task<List<SmartHealthCardVaccineInfo>> GetShcVaccineInfo(List<SmartHealthCardImmunization> Immunizations)
        {
            return Task.FromResult(new List<SmartHealthCardVaccineInfo>() {
                new SmartHealthCardVaccineInfo(),
                new SmartHealthCardVaccineInfo()
            });
        }
    }
}
