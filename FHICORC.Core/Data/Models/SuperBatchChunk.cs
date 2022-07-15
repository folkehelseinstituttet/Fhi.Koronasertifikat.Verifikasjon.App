using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FHICORC.Core.Data.Models
{
    public class SuperBatchChunk
    {
        public SuperBatchChunk(DateTime nextLastModified, IEnumerable<RevocationBatch> revocationBatches, bool hasMore = false)
        {
            N = nextLastModified;
            S = revocationBatches;
            M = hasMore;
        }

        // More revocationbatches available
        [JsonProperty("m")]
        public bool M { get; set; }

        /// <summary>
        /// NextLastModified
        /// The modified dateTime of the first Superbatch in the next chunk
        /// If ther there are noe more chunks to download this is set to null.
        /// </summary>
        [JsonProperty("n")]
        public DateTime N { get; set; }

        // SuperBatches
        [JsonProperty("s")]
        public IEnumerable<RevocationBatch> S { get; set; }
    }
}
