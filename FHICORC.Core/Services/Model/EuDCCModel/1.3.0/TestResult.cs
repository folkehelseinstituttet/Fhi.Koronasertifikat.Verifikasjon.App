using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Model.EuDCCModel._1._3._0
{
    public class TestResult
    {
        [JsonProperty("tg")]
        //disease or agent targeted
        public string Disease { get; set; }
        
        [JsonProperty("tt")]
        public string TypeOfTest { get; set; }

        [JsonProperty("nm")] 
        public string NAATestName { get; set; }
        
        [JsonProperty("ma")]
        public string TestManufacturer { get; set; }

        [JsonProperty("sc")]
        public DateTime? SampleCollectedTime { get; set; }
        
        [JsonProperty("tr")]
        public string ResultOfTest { get; set; }
        
        [JsonProperty("tc")]
        public string TestingCentre { get; set; }
        
        [JsonProperty("co")]
        public string CountryOfTest { get; set; }
        
        [JsonProperty("is")]
        public string CertificateIssuer { get; set; }
        
        [JsonProperty("ci")]
        public string CertificateId { get; set; }
    }
}