using FHICORC.Core.Services.Enum;
using Newtonsoft.Json;
using SQLite;
using System;

namespace FHICORC.Core.Data.Models
{
    public class RevocationBatch : DatabaseEntityBase
    {
        [MaxLength(16), JsonProperty("i")]
        public string BatchId { get; set; }

        [MaxLength(512), JsonProperty("c")]
        public string CountryISO3166 { get; set; }

        [JsonProperty("b")]
        public int BucketType { get; set; }

        [JsonProperty("f")]
        public byte[] BloomFilter { get; set; }

        [JsonProperty("h")]
        public HashTypeEnum HashType { get; set; }

        [JsonProperty("e")]
        public DateTime ExpirationDate { get; set; }
    }
}
