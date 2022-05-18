using System.Collections;
using System.Collections.Generic;
using System.Text;
using BloomFilter;
using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Model;

namespace FHICORC.Core.Services.Utils
{
    public static class MobileUtils
    {
        public static bool ContainsCertificateFilterMobile(string certificateIdentifierHash, IEnumerable<RevocationBatch> revocationBatches, BloomFilterBuckets bloomFilterBuckets)
        {
            //BloomFilterBuckets bloomFilterBuckets = FillBloomBuckets();

            var allHashFunctionIndicies_k = CalculateAllIndicies(certificateIdentifierHash, bloomFilterBuckets);
            return CheckFilterByCountry(allHashFunctionIndicies_k, revocationBatches, bloomFilterBuckets);
        }


        public static bool CheckFilterByCountry(List<int[]> allHashFunctionIndicies_k, IEnumerable<RevocationBatch> revocationBatches, BloomFilterBuckets bloomFilterBuckets)
        {

            //var superFilter = _coronapassContext.RevocationSuperFilter
            //    .Where(s => s.SuperCountry.Equals(country));

            foreach (var r in revocationBatches)
            {
                //var bucketId = BucketIdBasedOnBitLength(r.Bits.Length, bloomFilterBuckets);
                var bitVector = r.Bits; //new BitArray(r.SuperFilter);
                var contains = bitVector.Contains(allHashFunctionIndicies_k[r.BucketId]);

                if (contains)
                    return true;
            }

            return false;
        }




        public static bool Contains(this BitArray filter, int[] indicies)
        {
            foreach (int i in indicies)
            {
                if (!filter[i])
                    return false;
            }
            return true;
        }


        public static int[] HashData(byte[] data, int m, int k)
        {
            var hashFunction = HashFunction.Functions[HashMethod.Murmur3KirschMitzenmacher];
            return hashFunction.ComputeHash(data, m, k);
        }


        public static List<int[]> CalculateAllIndicies(string hashString, BloomFilterBuckets bloomFilterBuckets)
        {
            var allHashFunctionIndicies_k = new List<int[]>();
            foreach (var bucketItem in bloomFilterBuckets.Buckets)
            {
                var hashedIndicies = HashData(Encoding.UTF8.GetBytes(hashString), bucketItem.BitVectorLength_m, bucketItem.NumberOfHashFunctions_k);
                allHashFunctionIndicies_k.Add(hashedIndicies);
            };

            return allHashFunctionIndicies_k;
        }


    }
}
