using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Coding
{
    public class SmartHealthCardVaccineInfo
    {
        // Id matches VaccineCode Id.
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }
}
