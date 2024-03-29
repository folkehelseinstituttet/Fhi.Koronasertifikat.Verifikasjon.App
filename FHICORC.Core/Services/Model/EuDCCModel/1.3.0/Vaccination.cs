﻿using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Model.EuDCCModel._1._3._0
{
    public class Vaccination
    {
        [JsonProperty("tg")]
        //disease or agent targeted
        public string Disease { get; set; }
        
        [JsonProperty("vp")]
        //vaccine or prophylaxis
        public string VaccineProphylaxis { get; set; }
        
        [JsonProperty("mp")]
        //vaccine medicinal product
        public string VaccineMedicinalProduct { get; set; }
        
        [JsonProperty("ma")]
        //Marketing Authorization Holder - if no MAH present, then manufacturer
        public string Manufacturer { get; set; }
        
        [JsonProperty("dn")]
        public int DoseNumber { get; set; }
        
        [JsonProperty("sd")]
        public int TotalSeriesOfDose { get; set; }
        
        [JsonProperty("dt")]
        public string DateOfVaccination { get; set; }
        
        [JsonProperty("co")]
        public string CountryOfVaccination { get; set; }
        
        [JsonProperty("is")]
        public string CertificateIssuer { get; set; }
        
        [JsonProperty("ci")]
        public string CertificateId { get; set; }
    }
}