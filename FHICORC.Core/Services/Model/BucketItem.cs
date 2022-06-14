using System;
using System.Collections.Generic;
using System.Text;

namespace FHICORC.Core.Services.Model
{
    public class BucketItem
    {
        public BucketItem(int bucketId, int maxValue, int bitVectorLength_m, int numberOfHashFunctions_k)
        {
            this.BucketId = bucketId;
            this.MaxValue = maxValue;
            this.BitVectorLength_m = bitVectorLength_m;
            this.NumberOfHashFunctions_k = numberOfHashFunctions_k;
        }

        public BucketItem() { 
        
        }


        public int BucketId { get; set; }
        public int MaxValue { get; set; }
        public int BitVectorLength_m { get; set; }
        public int NumberOfHashFunctions_k { get; set; }

    }

}
