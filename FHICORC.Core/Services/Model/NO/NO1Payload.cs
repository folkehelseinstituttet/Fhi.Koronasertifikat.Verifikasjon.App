using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.NoDGCModel;

namespace FHICORC.Core.Services.Model.NO
{
    public class NO1Payload: ITokenPayload
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
        public HCertNOModel DGCPayloadData { get; set; }

        public DateTime? ExpiredDateTime() => exp;

        public DateTime? IssueDateTime() => iat;

        public class HCertNOModel
        {
            [JsonProperty("1")]
            public NODGCSchemaV1 DGC;

        }
    }
}