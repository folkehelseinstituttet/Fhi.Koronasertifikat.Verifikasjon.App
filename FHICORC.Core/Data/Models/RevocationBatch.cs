using FHICORC.Core.Services.Enum;
using Newtonsoft.Json;
using SQLite;
using System;

namespace FHICORC.Core.Data.Models
{
    public class RevocationBatch : DatabaseEntityBase
    {
        [Indexed, NotNull, MaxLength(16), JsonProperty("Id")]
        public string BatchId { get; set; }

        [NotNull, MaxLength(512), JsonProperty("countryISO3166")]
        public string CountryISO3166 { get; set; }

        [NotNull, JsonProperty("bloomFilter")]
        public byte[] BloomFilter { get; set; }

        [NotNull, JsonProperty("bucketType")]
        public int BucketType { get; set; }

        [NotNull]
        public HashTypeEnum HashType { get; set; }

        [NotNull]
        public DateTime ExpirationDate { get; set; }
    }
}
