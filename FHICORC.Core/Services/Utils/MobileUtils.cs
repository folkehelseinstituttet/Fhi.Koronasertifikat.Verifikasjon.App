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
        public static bool ContainsCertificateFilterMobile(string certificateIdentifierHash, string signatureHash, IEnumerable<RevocationBatch> revocationBatches, BloomFilterBuckets bloomFilterBuckets)
        {
            var allHashFunctionCertificateIdentifierIndicies_k = CalculateAllIndicies(certificateIdentifierHash, bloomFilterBuckets);
            var allHashFunctionSignatureIndicies_k = CalculateAllIndicies(signatureHash, bloomFilterBuckets);
            return CheckFilterByCountry(allHashFunctionCertificateIdentifierIndicies_k, allHashFunctionSignatureIndicies_k, revocationBatches, bloomFilterBuckets);
        }


        public static bool CheckFilterByCountry(List<int[]> allHashFunctionCertificateIdentifierIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, IEnumerable<RevocationBatch> revocationBatches, BloomFilterBuckets bloomFilterBuckets)
        {
            foreach (var r in revocationBatches)
            {
                var bitVector = new BitArray(r.BloomFilter);
                var containsCertificateIdentfier = bitVector.Contains(allHashFunctionCertificateIdentifierIndicies_k[r.BucketType]);
                var containsSignature = bitVector.Contains(allHashFunctionSignatureIndicies_k[r.BucketType]);

                if (containsCertificateIdentfier || containsSignature)
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
