using System.Threading.Tasks;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Core.Interfaces
{
    public interface ISmartHealthCardRepository
    {
        public Task<SmartHealthCardVaccineInfo> GetVaccineInfo(SmartHealthCardCoding[] vaccineCodes);
    }
}
