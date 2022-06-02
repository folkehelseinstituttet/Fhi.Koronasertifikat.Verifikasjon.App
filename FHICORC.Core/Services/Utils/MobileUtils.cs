﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            return CheckFilterByCountry(allHashFunctionCertificateIdentifierIndicies_k, allHashFunctionSignatureIndicies_k, revocationBatches);
        }


        public static bool CheckFilterByCountry(List<int[]> allHashFunctionCertificateIdentifierIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, IEnumerable<RevocationBatch> revocationBatches)
        {
            foreach (var r in revocationBatches)
            {
                var bitVector = new BitArray(r.BloomFilter);

                bool contains;
                if (r.HashType == Enum.HashTypeEnum.Signature)
                {
                    contains = bitVector.Contains(allHashFunctionSignatureIndicies_k[r.BucketType]);
                }
                else {
                    contains = bitVector.Contains(allHashFunctionCertificateIdentifierIndicies_k[r.BucketType]);
                }

                if (contains)
                    return true;
            }

            return false;
        }


        static async IAsyncEnumerable<bool> LoopThroughBatchesAsync(List<int[]> allHashFunctionCertificateIdentifierIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, List<RevocationBatch> revocationBatches, int revocationBatchCount, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {

            
            for (int i = 0; i < revocationBatchCount; i++)
            {
                
                var bitVector = new BitArray(revocationBatches[i].BloomFilter);

                bool contains;
                if (revocationBatches[i].HashType == Enum.HashTypeEnum.Signature)
                {
                    contains = bitVector.Contains(allHashFunctionSignatureIndicies_k[revocationBatches[i].BucketType]);
                }
                else
                {
                    contains = bitVector.Contains(allHashFunctionCertificateIdentifierIndicies_k[revocationBatches[i].BucketType]);
                }


                await Task.Delay(i, cancellationToken);

                if (contains)
                    yield return true;


                yield return false;

            }
        }


        public async static Task<bool> CheckFilterByCountryAsync(List<int[]> allHashFunctionCertificateIdentifierIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, IEnumerable<RevocationBatch> revocationBatches)
        {

            var cts = new CancellationTokenSource();
            var revocationBatchCount = revocationBatches.ToList().Count();


            await foreach (bool contains in LoopThroughBatchesAsync(allHashFunctionCertificateIdentifierIndicies_k, allHashFunctionSignatureIndicies_k, revocationBatches.ToList(), revocationBatchCount).WithCancellation(cts.Token)) {
                if (contains) {
                    cts.Cancel();
                    return true;
                }
                    
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