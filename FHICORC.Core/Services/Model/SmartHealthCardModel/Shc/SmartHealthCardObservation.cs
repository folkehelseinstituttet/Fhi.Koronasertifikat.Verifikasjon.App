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

        [JsonProperty("valueQuantity")]
        public ValueQuantity ValueQuantity { get; set; }

        [JsonProperty("valueString")]
        public string ValueString { get; set; }

        [JsonProperty("valueCodeableConcept")]
        public CodeableConcept ValueCodeableConcept { get; set; }
    }

    public class Subject
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }

    public class ValueQuantity
    {
        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("comparator")]
        public string Comparator { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
