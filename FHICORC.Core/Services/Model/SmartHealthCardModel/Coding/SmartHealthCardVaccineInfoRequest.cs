using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Coding
{
    public class SmartHealthCardVaccineInfoRequest
    {
        [JsonProperty("codes")]
        public SmartHealthCardCoding[] Codes { get; set; }

        public SmartHealthCardVaccineInfoRequest(SmartHealthCardCoding[] Coding)
        {
            Codes = Coding;
        }
    }
}
