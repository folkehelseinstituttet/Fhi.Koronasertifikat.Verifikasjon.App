using Newtonsoft.Json;
using SQLite;

namespace FHICORC.Models.DataModels
{
    public class RevocationBatch : DatabaseEntityBase
    {
        [Indexed, NotNull, MaxLength(16), JsonProperty("batchId")]
        public string BatchId { get; set; }

        [NotNull, MaxLength(512), JsonProperty("country")]
        public string Country { get; set; }
    }
}
