/*using FHICORC.Configuration;
using NUnit.Framework;
using System;
using FHICORC.Core.Data;
using FHICORC.Core.Data.Models;
using FHICORC.Core.Interfaces;
using FHICORC.Data;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FHICORC.Core.Services.DecoderServices;
using FHICORC.Tests.TestMocks;
using FHICORC.Core.Services.Utils;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Enum;

namespace FHICORC.Tests.RevocationTests
{
    public class CheckRevocationTests
    {
        private List<RevocationBatch> revocationBatchtes;


        [OneTimeSetUp]
        public void SetUp()
        {

            revocationBatchtes = JsonConvert.DeserializeObject<List<RevocationBatch>>(File.ReadAllText("RevocationDownloadReponse200.json"));

        }

        //[TestCase("RO", "", "J4PCK4sFs63kH/EeP7+C3A==")]
        [TestCase("CZ", "+hkApE6qvfhfb95y/Jjx/w==", "+hkApE6qvfhfb95y/Jjx/w==", true)]
        [TestCase("CZ", "+hkApE6qvfhfb95y/Jjx/w==", "+hkApE6qvfhfb95y/Jjx/w==", false)]
        public void CheckIfSingleRevocationExists(string isoCode, string uciHash, string signatureHash, bool isParallel=false) {

            var revocationBatchesCountry = GetRevocationBatchesFromCountry(isoCode);

            var sut = new CertificateRevocationService(new MockRevocationBatchService());

            var result = sut.CheckHashInRevocationBatchesAsync(revocationBatchesCountry, uciHash, uciHash, signatureHash, isParallel);

            Assert.True(result);

        }



        [Test]
        public void CheckAllRevocationExists() {

            var revocationHashes = new List<string>(File.ReadAllLines("RevocationHash200.txt"));
            var isoCodes = new List<string>() { "CZ", "DE", "DX", "ES", "FR", "HR", "IT", "RO", "XX", "YA" };
            var sut = new CertificateRevocationService(new MockRevocationBatchService());

            foreach (var revocationHash in revocationHashes) {

                var uciOrSingatureHash = revocationHash.Replace("\t", "");
                var doesHashExistInRevocationBloomFilters = false;

                foreach (var isoCode in isoCodes) {
                    var revocationBatchesCountry = GetRevocationBatchesFromCountry(isoCode);
                    var result = sut.CheckHashInRevocationBatchesAsync(revocationBatchesCountry, uciOrSingatureHash, uciOrSingatureHash, uciOrSingatureHash);

                    if (result) {
                        doesHashExistInRevocationBloomFilters = true;
                        break;
                    }
                }

                Assert.True(doesHashExistInRevocationBloomFilters);

            }
        }


        [TestCase("CZ", "+hkApE6qvfhfb95y/Jjx/w==", "+hkApE6qvfhfb95y/Jjx/w==")]
        public void CheckPerformanceIfLocalDatabaseIsBigAFandRevocedPassIsLast(string isoCode, string uciHash, string signatureHash) {

            var watch = new System.Diagnostics.Stopwatch();

            AddJunkBatches(isoCode);

            watch.Start();
            CheckIfSingleRevocationExists(isoCode, uciHash, signatureHash, false);
            watch.Stop();
            TestContext.WriteLine($"Non-parallel Execution Time: {watch.ElapsedMilliseconds} ms");

            var watch2 = new System.Diagnostics.Stopwatch();
            watch2.Start();
            CheckIfSingleRevocationExists(isoCode, uciHash, signatureHash, true);
            watch2.Stop();
            TestContext.WriteLine($"Parallel Execution Time: {watch2.ElapsedMilliseconds} ms");

        }

        private List<RevocationBatch> AddJunkBatches(string isoCode, int amountOfJunkBatches=80000){

            var buckets = MobileUtils.FillBloomBuckets();
            Random random = new Random();


            for (var i = 0; i < amountOfJunkBatches; i++)
            {
                //int numberOfHashesInBatch = random.Next(1, 999);
                int numberOfHashesInBatch = 1000;

                BucketItem bucketItem = new BucketItem();
                foreach (var b in buckets)
                {
                    bucketItem = b;
                    if (b.MaxValue >= numberOfHashesInBatch)
                        break;
                }

                var mockedBatch = new RevocationBatch()
                {
                    BatchId = "",
                    CountryISO3166 = isoCode,
                    BloomFilter = GetByteArray(bucketItem.BitVectorLength_m / 8),
                    BucketType = bucketItem.BucketId,
                    HashType = HashTypeEnum.Signature,
                };

                revocationBatchtes.Insert(0, mockedBatch);
            }

            return revocationBatchtes;

        }

        private byte[] GetByteArray(int byteSize)
        {
            Random rnd = new Random();
            byte[] b = new byte[byteSize];
            rnd.NextBytes(b);
            return b;
        }




        private List<RevocationBatch> GetRevocationBatchesFromCountry(string isoCode) {
            return revocationBatchtes.Where(rb => rb.CountryISO3166 == isoCode).ToList();
        }
    }
}
*/