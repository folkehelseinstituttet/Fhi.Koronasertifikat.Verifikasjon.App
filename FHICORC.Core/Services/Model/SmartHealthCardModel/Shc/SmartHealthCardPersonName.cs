using System;
using System.Linq;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class SmartHealthCardPersonName
    {
        [JsonProperty("family")]
        public string FamilyName { get; set; }

        [JsonProperty("given")]
        public string[] GivenName { get; set; }

        public string FullName => $"{FamilyName}, {string.Join(" ", GivenName)}";
    }
}