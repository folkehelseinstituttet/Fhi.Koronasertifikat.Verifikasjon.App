using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class BusinessRule
    {
        public string Identifier { get; set; }
        public string Version { get; set; }
        public string SchemaVersion { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }
        public string Type { get; set; }
        public string CertificateType { get; set; }
        public ICollection<RuleDescriptionTranslation> Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public ICollection<string> AffectedFields { get; set; }
        public object Logic { get; set; }
    }
}
