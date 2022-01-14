using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;

namespace FHICORC.Core.Services.DecoderService
{
    public class CodingService : ICodingService
    {
        private readonly ISmartHealthCardRepository _smartHealthCardRepository;

        public CodingService(ISmartHealthCardRepository smartHealthCardRepository)
        {
            _smartHealthCardRepository = smartHealthCardRepository;
        }

        public async Task<List<SmartHealthCardVaccineInfo>> GetShcVaccineInfo(List<SmartHealthCardImmunization> Immunizations)
        {
            List<SmartHealthCardVaccineInfo> info = new List<SmartHealthCardVaccineInfo>();

            // Call api sequentially, some problems with shared http client if running in parallel
            foreach (CodeableConcept vaccineCode in Immunizations.Select(x => x.VaccineCode).Distinct())
            {
                SmartHealthCardVaccineInfo vaccineInfo = await _smartHealthCardRepository.GetVaccineInfo(vaccineCode.Coding);
                Debug.Print($"{nameof(CodingService)}: Got vaccine info for vaccine: " +
                    $"{vaccineInfo.Name}, {vaccineInfo.Type}");
                vaccineInfo.Id = vaccineCode.Id;
                info.Add(vaccineInfo);
            };

            return info;
        }
    }
}
