using FHICORC.Core.Services.Enum;
using Newtonsoft.Json;
using SQLite;
using System;

namespace FHICORC.Core.Data.Models
{
    public class RevocationBatch : DatabaseEntityBase
    {
        [Indexed, NotNull, MaxLength(16), JsonProperty("I")]
        public string BatchId { get; set; }

        [NotNull, MaxLength(512), JsonProperty("C")]
        public string CountryISO3166 { get; set; }

        [NotNull, JsonProperty("B")]
        public byte[] BloomFilter { get; set; }

        [NotNull, JsonProperty("F")]
        public int BucketType { get; set; }

        [NotNull, JsonProperty("H")]
        public HashTypeEnum HashType { get; set; }

        [NotNull, JsonProperty("E")]
        public DateTime ExpirationDate { get; set; }
    }
}
