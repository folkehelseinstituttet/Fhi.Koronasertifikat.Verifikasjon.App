using System;
using System.Collections.Generic;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class VerifyRulesModel
    {
        [JsonProperty("payload")]
        public DCCSchemaV1 HCert { get; set; }
        [JsonProperty("external")]
        public ExternalData External { get; set; }
    }

    public class ExternalData
    {
        [JsonProperty("validationClock")]
        public DateTime ValidationClock { get; set; }
        public ValueSets ValueSets { get; set; }
        public string CountryCode { get; set; }
        public DateTime? Exp { get; set; }
        public DateTime? Iat { get; set; }
    }

    public class ValueSets
    {
        [JsonProperty("disease-agent-targeted")]
        public List<string> DiseaseAgentTargeted { get; set; }

        [JsonProperty("covid-19-lab-result")]
        public List<string> Covid19LabResult { get; set; }

        [JsonProperty("vaccines-covid-19-auth-holders")]
        public List<string> VaccinesCovid19AuthHolders { get; set; }

        [JsonProperty("vaccines-covid-19-names")]
        public List<string> VaccinesCovid19Names { get; set; }

        [JsonProperty("sct-vaccines-covid-19")]
        public List<string> SctVaccinesCovid19 { get; set; }

        [JsonProperty("covid-19-lab-test-manufacturer-and-name")]
        public List<string> TestManufacturers { get; set; }

        [JsonProperty("covid-19-lab-test-type")]
        public List<string> TestTypes { get; set; }
    }
}
