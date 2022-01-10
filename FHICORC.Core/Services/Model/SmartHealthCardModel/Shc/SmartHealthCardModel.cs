using System;
using System.Globalization;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardModel : ITokenPayload
    {

        [JsonConstructor]
        public SmartHealthCardModel(Uri Issuer, DateTime IssuanceDate, VerifiableCredential VerifiableCredential)
        {
            this.Issuer = Issuer;
            this.IssuanceDate = IssuanceDate;
            this.VerifiableCredential = VerifiableCredential;
        }

        [JsonProperty("iss", Required = Required.Always)]
        public Uri Issuer { get; set; }

        [JsonProperty("nbf", Required = Required.Default)]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? IssuanceDate { get; set; }

        [JsonProperty("vc", Required = Required.Always)]
        public VerifiableCredential VerifiableCredential { get; set; }

        public DateTime? ExpiredDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime? IssueDateTime() => IssuanceDate;
    }
}
