using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer
{
    public class SmartHealthCardIssuerRequest
    {
        public SmartHealthCardIssuerRequest(string issuer)
        {
            Issuer = issuer;
        }

        [JsonProperty("iss")]
        public string Issuer { get; set; }
    }
}
