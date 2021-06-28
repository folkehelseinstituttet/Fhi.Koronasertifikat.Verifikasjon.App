using System;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.EuDCCModel._1._3._0
{
    public class DCCSchemaV1
    {
        [JsonProperty("ver")]
        public string Version { get; set; }

        [JsonProperty("nam")]
        public PersonName PersonName { get; set; }
        
        [JsonProperty("dob")]
        public string DateOfBirth { get; set; }

        [JsonProperty("v")]
        public Vaccination[] Vaccinations { get; set; }
        [JsonProperty("t")]
        public TestResult[] Tests { get; set; }
        [JsonProperty("r")]
        public Recovery[] Recovery { get; set; }
    }
}