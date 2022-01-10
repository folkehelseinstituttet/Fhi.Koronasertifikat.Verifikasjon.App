using System;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardObservation
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public CodeableConcept TestCode { get; set; }

        [JsonProperty("subject")]
        public Subject Subject { get; set; }

        [JsonProperty("effectiveDateTime")]
        [JsonConverter(typeof(ISO8601DateTimeConverter))]
        public DateTime? EffectiveDateTime { get; set; }
    }

    public class Subject
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
