using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Model.EuDCCModel;
using FHICORC.Core.Services.Model.Converter;

namespace FHICORC.Core.Services.Model.NoDGCModel
{
    public class NODGCSchemaV1
    {
        [JsonProperty("ver")]
        public string Version { get; set; }

        [JsonProperty("nam")]
        public PersonName PersonName { get; set; }

        [JsonProperty("dob")]
        public string DateOfBirth { get; set; }
    }
}