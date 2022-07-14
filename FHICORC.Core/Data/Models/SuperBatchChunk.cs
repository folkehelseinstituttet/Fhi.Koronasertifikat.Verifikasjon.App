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
        public bool M { get; private set; }

        /// <summary>
        /// NextLastModified
        /// The modified dateTime of the first Superbatch in the next chunk
        /// If ther there are noe more chunks to download this is set to null.
        /// </summary>
        public DateTime N { get; private set; }

        // SuperBatches
        public IEnumerable<RevocationBatch> S { get; private set; }
    }
}
