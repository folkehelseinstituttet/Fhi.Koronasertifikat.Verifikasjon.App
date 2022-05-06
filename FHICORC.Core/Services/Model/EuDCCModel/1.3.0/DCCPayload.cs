using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;

namespace FHICORC.Core.Services.Model.EuDCCModel._1._3._0
{
    public class DCCPayload: ITokenPayload
    {
        [JsonProperty("1")]
        public string Issuer { get; set; }

        [JsonProperty("6")]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? IssueAt { get; set; }

        [JsonProperty("4")]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? ExpirationTime { get; set; }

        [JsonProperty("-260")]
        public HCertModel DCCPayloadData { get; set; }

        public DateTime? ExpiredDateTime() => ExpirationTime;

        public DateTime? IssueDateTime() => IssueAt;
    }
    public class HCertModel
    {
        [JsonProperty("1")]
        public DCCSchemaV1 DCC;
    }
}