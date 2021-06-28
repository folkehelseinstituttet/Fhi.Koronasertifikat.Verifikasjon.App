using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.EuDCCModel
{
    public class PersonName
    {
        [JsonProperty("gn")]
        public string Forename { get; set; }
        [JsonProperty("gnt")]
        public string StandardisedForename { get; set; }
        [JsonProperty("fn")]
        public string Surname { get; set; }
        [JsonProperty("fnt")]
        public string StandardisedSurname { get; set; }

        public string FullName => $"{StandardisedSurname}, {StandardisedForename}";
    }
}