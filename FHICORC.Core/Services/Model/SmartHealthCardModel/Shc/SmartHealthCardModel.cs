using System;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardModel : ITokenPayload
    {
        [JsonProperty("iss", Required = Required.Always)]
        public Uri Issuer { get; set; }

        [JsonProperty("nbf", Required = Required.Default)]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? IssuanceDate { get; set; }

        [JsonProperty("vc", Required = Required.Always)]
        public VerifiableCredential VerifiableCredential { get; set; }

        [JsonProperty("exp")]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? ExpirationDate { get; set; }

        public DateTime? ExpiredDateTime() => ExpirationDate;

        public DateTime? IssueDateTime() => IssuanceDate;
    }
}
