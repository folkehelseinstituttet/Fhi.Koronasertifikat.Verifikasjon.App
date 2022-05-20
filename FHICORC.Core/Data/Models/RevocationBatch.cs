using Newtonsoft.Json;
using SQLite;

namespace FHICORC.Core.Data.Models
{
    public class RevocationBatch : DatabaseEntityBase
    {
        [Indexed, NotNull, MaxLength(16), JsonProperty("batchId")]
        public string BatchId { get; set; }

        [NotNull, MaxLength(512), JsonProperty("country")]
        public string Country { get; set; }

        [NotNull, JsonProperty("superFilter")]
        public byte[] Bits { get; set; }

        [NotNull, JsonProperty("bucketId")]
        public int BucketId { get; set; }
    }
}
