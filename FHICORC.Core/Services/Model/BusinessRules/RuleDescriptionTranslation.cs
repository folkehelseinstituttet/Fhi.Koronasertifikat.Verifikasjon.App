using System;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class RuleDescriptionTranslation
    {
        [JsonProperty("lang")]
        public string LanguageCode { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
    }
}
