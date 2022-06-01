using FHICORC.Configuration;
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

namespace FHICORC.Tests.RevocationTests
{
    public class CheckRevocationTests
    {
        private List<RevocationBatch> revocationBatchtes;


        [OneTimeSetUp]
        public void SetUp()
        {
            revocationBatchtes = JsonConvert.DeserializeObject<List<RevocationBatch>>(File.ReadAllText("RevocationDownloadReponse.json"));

        }

        [TestCase("RO", "", "J4PCK4sFs63kH/EeP7+C3A==")]
        public void CheckIfSingleRevocationExists(string isoCode, string uciHash, string signatureHash) {

            var revocationBatchesCountry = GetRevocationBatchesFromCountry(isoCode);

            var sut = new CertificateRevocationService(new MockRevocationBatchService());

            var result = sut.CheckHashInRevocationBatchesAsync(revocationBatchesCountry, uciHash, signatureHash);

            Assert.True(result);

        }

        private List<RevocationBatch> GetRevocationBatchesFromCountry(string isoCode) {
            return revocationBatchtes.Where(rb => rb.CountryISO3166 == isoCode).ToList();
        }
    }
}
