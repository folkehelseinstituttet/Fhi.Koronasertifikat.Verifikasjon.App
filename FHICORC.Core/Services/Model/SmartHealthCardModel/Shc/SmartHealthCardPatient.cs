using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardPatient
    {
        [JsonProperty("name")]
        internal SmartHealthCardPersonName[] PersonNames { get; set; }

        [JsonIgnore]
        public SmartHealthCardPersonName PersonName
        {
            get
            {
                return PersonNames[0];
            }
        }

        [JsonProperty("birthDate")]
        public string DateOfBirth { get; set; }
    }
}
