using System;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class BusinessRule
    {
        [JsonProperty("id")]
        public string Identifier { get; set; }
        public string Version { get; set; }
        public string SchemaVersion { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }
        public string Type { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        [JsonProperty("inputParameter")]
        public string AffectedFields { get; set; }
        [JsonProperty("logic")]
        public object Logic { get; set; }
    }
}
