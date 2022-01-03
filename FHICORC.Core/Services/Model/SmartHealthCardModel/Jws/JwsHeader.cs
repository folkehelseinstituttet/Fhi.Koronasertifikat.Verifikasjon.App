﻿using Newtonsoft.Json;

#nullable enable

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Jws
{
    /// <summary>
    /// Parameter names found in RFC 7515, see https://tools.ietf.org/html/rfc7515
    /// </summary>
    public class JwsHeader
    {
        [JsonProperty("typ")]
        public string? Typ { get; set; }

        [JsonProperty("cty")]
        public string? Cty { get; set; }

        [JsonProperty("alg")]
        public string? Alg { get; set; }

        [JsonProperty("kid")]
        public string? Kid { get; set; }

        [JsonProperty("x5u")]
        public string? X5u { get; set; }

        [JsonProperty("x5c")]
        public string[]? X5c { get; set; }

        [JsonProperty("x5t")]
        public string? X5t { get; set; }

        [JsonProperty("zip")]
        public string? Zip { get; set; }
    }
}
