using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BloomFilter;
using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Model;

namespace FHICORC.Core.Services.Utils
{
    public static class MobileUtils
    {
        public static bool ContainsCertificateFilterMobile(string uciHash, string countrycodeuciHash, string signatureHash, IEnumerable<RevocationBatch> revocationBatches, List<BucketItem> bloomFilterBuckets, bool isParallel=true)
        {
            var allHashFunctionUci_k = CalculateAllIndicies(uciHash, bloomFilterBuckets);
            var allHashFunctionCountrycodeuci = CalculateAllIndicies(countrycodeuciHash, bloomFilterBuckets);
            var allHashFunctionSignatureIndicies_k = CalculateAllIndicies(signatureHash, bloomFilterBuckets);

            if(isParallel)
                return CheckFilterByCountryParallel(allHashFunctionUci_k, allHashFunctionCountrycodeuci, allHashFunctionSignatureIndicies_k, revocationBatches);

            return CheckFilterByCountry(allHashFunctionUci_k, allHashFunctionCountrycodeuci, allHashFunctionSignatureIndicies_k, revocationBatches);
        }


        public static bool CheckFilterByCountry(List<int[]> allHashFunctionUciIndicies_k, List<int[]> allHashFunctionCountrycodeuciIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, IEnumerable<RevocationBatch> revocationBatches)
        {
            foreach (var r in revocationBatches)
            {
                var contains = BatchContains(r, allHashFunctionUciIndicies_k, allHashFunctionCountrycodeuciIndicies_k, allHashFunctionSignatureIndicies_k);
                if (contains)
                    return true;
            }

            return false;
        }


        public static bool CheckFilterByCountryParallel(List<int[]> allHashFunctionUciIndicies_k, List<int[]> allHashFunctionCountrycodeuciIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k, IEnumerable<RevocationBatch> revocationBatches)
        {

            bool contains = revocationBatches.AsParallel()
                .Any(r => BatchContains(r, allHashFunctionUciIndicies_k, allHashFunctionCountrycodeuciIndicies_k, allHashFunctionSignatureIndicies_k));
            return contains;
        }

        public static bool BatchContains(RevocationBatch revocationBatch, List<int[]> allHashFunctionUciIndicies_k, List<int[]> allHashFunctionCountrycodeuciIndicies_k, List<int[]> allHashFunctionSignatureIndicies_k)
        {

            var bitVector = new BitArray(revocationBatch.BloomFilter);

            bool contains;
            if (revocationBatch.HashType == Enum.HashTypeEnum.Signature)
            {
                contains = bitVector.Contains(allHashFunctionSignatureIndicies_k[revocationBatch.BucketType]);
            }
            //else if (revocationBatch.HashType == Enum.HashTypeEnum.UCICountry)
            //{
            //    contains = bitVector.Contains(allHashFunctionCountrycodeuciIndicies_k[revocationBatch.BucketType]);
            //}
            else
            {
                contains = bitVector.Contains(allHashFunctionUciIndicies_k[revocationBatch.BucketType]);
            }

            return contains;

            //var bitVector = new BitArray(revocationBatch.BloomFilter);

            //bool contains;
            //if (revocationBatch.HashType == Enum.HashTypeEnum.Signature)
            //{
            //    contains = bitVector.Contains(allHashFunctionSignatureIndicies_k[revocationBatch.BucketType]);
            //}
            //else
            //{
            //    contains = bitVector.Contains(allHashFunctionCertificateIdentifierIndicies_k[revocationBatch.BucketType]);
            //}

            //return contains;
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


        public static List<int[]> CalculateAllIndicies(string hashString, List<BucketItem> bloomFilterBuckets)
        {
            var allHashFunctionIndicies_k = new List<int[]>();
            foreach (var bucketItem in bloomFilterBuckets)
            {
                var hashedIndicies = HashData(Encoding.UTF8.GetBytes(hashString), bucketItem.BitVectorLength_m, bucketItem.NumberOfHashFunctions_k);
                allHashFunctionIndicies_k.Add(hashedIndicies);
            };

            return allHashFunctionIndicies_k;
        }

        public static List<BucketItem> FillBloomBuckets()
        {
            var numberOfBuckets = 200;
            var minValue = 5;
            var maxValue = 1000;
            var stepness = 1;
            var falsePositiveProbability = 1e-10;

            //var numberOfBuckets = 10;
            //var minValue = 5;
            //var maxValue = 1000;
            //var stepness = 2.5;
            //var falsePositiveProbability = 1e-10;

            var bloomFilterBucketsList = new List<BucketItem>();

            for (var i = 0; i < numberOfBuckets; i++)
            {
                var bucketValue = (int)Math.Ceiling((maxValue - minValue) /
                    (Math.Pow(numberOfBuckets - 1, stepness))
                    * Math.Pow(i, stepness) + minValue);

                var bloomStats = BloomFilterUtils.CalcOptimalMK(bucketValue, falsePositiveProbability);
                var bucketItem = new BucketItem(i, bucketValue, bloomStats.m, bloomStats.k);
                bloomFilterBucketsList.Add(bucketItem);

            }

            return bloomFilterBucketsList;
        }

    }
}
