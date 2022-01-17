using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer
{
    public class SmartHealthCardIssuer
    {
        [JsonProperty("trusted")]
        public bool Trusted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
