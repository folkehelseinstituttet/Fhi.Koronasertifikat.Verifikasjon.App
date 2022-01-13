﻿using System;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardImmunization
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("vaccineCode")]
        public CodeableConcept VaccineCode { get; set; }

        [JsonProperty("patient")]
        public Patient Patient { get; set; }

        [JsonProperty("occurrenceDateTime")]
        [JsonConverter(typeof(ISO8601DateTimeConverter))]
        public DateTime? OccurrenceDateTime { get; set; }
    }

    public class CodeableConcept
    {
        [JsonProperty("coding")]
        public SmartHealthCardCoding[] Coding { get; set; }
    }

    public class Patient
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}