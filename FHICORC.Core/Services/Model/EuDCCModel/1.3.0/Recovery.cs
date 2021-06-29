using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Model.EuDCCModel._1._3._0
{
    public class Recovery
    {
        [JsonProperty("tg")]
        //disease or agent targeted
        public string Disease { get; set; }
        
        [JsonProperty("fr")]
        public string DateOfFirstPositiveResult{ get; set; }
        
        [JsonProperty("co")]
        public string CountryOfTest { get; set; }
        
        [JsonProperty("is")]
        public string CertificateIssuer { get; set; }
        
        [JsonProperty("ci")]
        public string CertificateId { get; set; }

        [JsonProperty("df")]
        public string ValidFrom{ get; set; }

        [JsonProperty("du")]
        public string ValidTo{ get; set; }
    }
}