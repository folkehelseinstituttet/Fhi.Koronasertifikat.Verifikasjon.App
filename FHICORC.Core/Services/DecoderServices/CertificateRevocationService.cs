using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using FHICORC.Core.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.DecoderServices
{
    public class CertificateRevocationService : ICertificateRevocationService
    {
        private static readonly Dictionary<string, Func<string, string>> _customGetCertificateIdentifierHashFunctionDirectory = new Dictionary<string, Func<string, string>>()
        {
            { "BE", (x) => x.Split('#')[0] },                       // "01CountryOpaqueUniqueString#Checksum"
            { "CY", (x) => x.Split('#')[0].Split(':')[3] },         // "dgci:V1:Country:OpaqueUniqueString#Checksum"
            { "DE", (x) => x.Split('#')[0].Split(':', '/')[4] },    // "URN:UVCI:01Country/IssuingEntity/OpaqueUniqueString#Checksum"
            { "ES", (x) => x.Split('#')[0] },                       // "01CountryOpaqueUniqueString#Checksum"
            { "FR", (x) => x.Split('#')[0].Split(':')[3] },         // "dgci:V1:Country:OpaqueUniqueString#Checksum"
            { "IT", (x) => x.Split('#')[0] },                       // "01CountryOpaqueUniqueString#Checksum"
            { "LU", (x) => x.Split('#')[0].Split('/')[2] },         // "01/Country/OpaqueUniqueString#Checksum"
            { "PT", (x) => x.Split('#')[0].Split(':', '/')[5] },    // "urn:uvci:01/Country/SPMS/OpaqueUniqueString#Checksum"
            { "EE", (x) => x.Split('#')[0].Split('/')[3] }          // "01/Country/TIS/OpaqueUniqueString#Checksum"
        };

        private readonly IRevocationBatchService _revocationBatchService;
        private readonly BloomFilterBuckets _bloomFilterBuckets;

        public CertificateRevocationService(IRevocationBatchService revocationBatchService)
        {
            _revocationBatchService = revocationBatchService;
            _bloomFilterBuckets = FillBloomBuckets();
        }

        public static BloomFilterBuckets FillBloomBuckets()
        {
            var buckets = new List<BucketItem>()
            {
                new BucketItem(){ BucketId = 0, BitVectorLength_m = 240, MaxValue = 5, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 1, BitVectorLength_m = 480, MaxValue = 10, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 2, BitVectorLength_m = 1390, MaxValue = 29, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 3, BitVectorLength_m = 3307, MaxValue = 69, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 4, BitVectorLength_m = 6566, MaxValue = 137, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 5, BitVectorLength_m = 11215, MaxValue = 234, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 6, BitVectorLength_m = 17589, MaxValue = 367, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 7, BitVectorLength_m = 25688, MaxValue = 536, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 8, BitVectorLength_m = 35801, MaxValue = 747, NumberOfHashFunctions_k = 34},
                new BucketItem(){ BucketId = 9, BitVectorLength_m = 47926, MaxValue = 1000, NumberOfHashFunctions_k = 34},

            };

            return new BloomFilterBuckets() { Buckets = buckets };
        }


        public async Task<bool> IsCertificateRevoked(ITokenPayload token)
        {
            if(token is DCCPayload payload)
            {
                (var certificateIdentifier, var isoCode) = GetCertificateIdentifierAndISOCodeFromTokenPayload(payload);
                var certificateIdentifierHash = GetCertificateIdentifierHashFromCertificateIdentifier(certificateIdentifier, isoCode);
                var revocationBatches = await _revocationBatchService.GetRevocationBatchesFromCountry(isoCode);
                return CheckHashInRevocationBatches(revocationBatches, certificateIdentifierHash);
            }
            return false;
        }

        private (string certificateIdentifier, string isoCode) GetCertificateIdentifierAndISOCodeFromTokenPayload(DCCPayload payload)
        {
            var obj = payload.DCCPayloadData.DCC;

            if (obj?.Vaccinations.Any() ?? false)
                return (obj.Vaccinations.First().CountryOfVaccination, obj.Vaccinations.First().CertificateId);
            else if (obj?.Tests.Any() ?? false)
                return (obj.Tests.First().CountryOfTest, obj.Tests.First().CertificateId);
            else if (obj?.Recovery.Any() ?? false)
                return (obj.Recovery.First().CountryOfTest, obj.Recovery.First().CertificateId);
            else
                throw new ArgumentException($"DCCPayload did not contain any DCC result data.");
        }

        private string GetCertificateIdentifierHashFromCertificateIdentifier(string certificateIdentifier, string isoCode)
        {
            var certificateIdentifierWithoutCheckSum = certificateIdentifier.Split('#').First();
            var colons = certificateIdentifierWithoutCheckSum.Count(x => x.Equals(':'));
            var slash = certificateIdentifierWithoutCheckSum.Count(x => x.Equals('/'));

            return (slash, colons) switch 
            {
                (0, 4) => certificateIdentifierWithoutCheckSum.Split(':')[4], // Unique Vaccination Certificate Identifier(UVCI) Option 2 - opaque identifier - no structure
                (1, 4) => certificateIdentifierWithoutCheckSum.Split(':')[4], // Unique Vaccination Certificate Identifier(UVCI) Option 3 - some semantics
                (2, 4) => certificateIdentifierWithoutCheckSum.Split(':')[4], // Unique Vaccination Certificate Identifier(UVCI) Option 1 - identifier with semantics
                _ => _customGetCertificateIdentifierHashFunctionDirectory.TryGetValue(isoCode, out var countryGetCertficateIdentifierHashFunction) switch
                {
                    true => countryGetCertficateIdentifierHashFunction.Invoke(certificateIdentifier),
                    _ => throw new ArgumentException($"No GetCertficateIdentifierHashFunction found for specified country: {isoCode}. The certificate identifier is not in a valid format.")
                }  
            };
        }

        private bool CheckHashInRevocationBatches(IEnumerable<RevocationBatch> revocationBatches, string certificateIdentifierHash)
        {

            //var result = MobileUtils.ContainsCertificateFilterMobile(certificateIdentifierHash, revocationBatches, _bloomFilterBuckets);
            //return result;

            //int m = 0;
            //int k = 0;

            //foreach (var batch in revocationBatches)
            //{
            //    // If any hash functions hits a bit with a zero, go to next batch (contiune)


            //    if (batch.Bits.Contains(certificateIdentifierHash, m, k))
            //        return true;
            //}

            return false;
        }
    }
}
