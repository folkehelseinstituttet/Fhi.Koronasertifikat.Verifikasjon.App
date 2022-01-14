using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Core.Services.Interface
{
    public interface ICodingService
    {
        public Task<List<SmartHealthCardVaccineInfo>> GetShcVaccineInfo(List<SmartHealthCardImmunization> Immunizations);
    }
}
