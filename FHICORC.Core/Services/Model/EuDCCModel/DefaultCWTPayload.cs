using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;

namespace FHICORC.Core.Services.Model.EuDCCModel
{
    public class DefaultCWTPayload: ITokenPayload
    {
        [JsonProperty("1")]
        public String iss { get; set; }

        [JsonProperty("6")]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? iat { get; set; }

        [JsonProperty("4")]
        [JsonConverter(typeof(EpochDatetimeConverter))]
        public DateTime? exp { get; set; }
        
        [JsonProperty("-260")]
        public HCertModel DGCPayloadData { get; set; }
        public DateTime? ExpiredDateTime() => exp;

        public DateTime? IssueDateTime() => iat;
    }
    public class HCertModel
    {
        [JsonProperty("1")]
        public DefaultDgcSchema euHcertV1Schema;

    }

    public class DefaultDgcSchema
    {
        [JsonProperty("ver")]
        public string Version { get; set; }
    }
}