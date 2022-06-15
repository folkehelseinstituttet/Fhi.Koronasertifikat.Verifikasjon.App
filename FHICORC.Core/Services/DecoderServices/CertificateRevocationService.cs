using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using FHICORC.Core.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

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
        private readonly List<BucketItem> _bloomFilterBuckets;

        public CertificateRevocationService(IRevocationBatchService revocationBatchService)
        {
            _revocationBatchService = revocationBatchService;
            _bloomFilterBuckets = MobileUtils.FillBloomBuckets();
        }


        public async Task<bool> IsCertificateRevoked(ITokenPayload token, string signatureBase64EncodedHash)
        {
            if(token is DCCPayload payload)
            {
                (var isoCode, var certificateIdentifier) = GetCertificateIdentifierAndISOCodeFromTokenPayload(payload);
                var certificateIdentifierHash = GetCertificateIdentifierHashFromCertificateIdentifier(certificateIdentifier, isoCode);
                var revocationBatches = await _revocationBatchService.GetRevocationBatchesFromCountry(isoCode);
                var certificateIdentifierBase64EndodedHash = Base64EncodeCertificateIdentifierHash(certificateIdentifierHash);
                return CheckHashInRevocationBatchesAsync(revocationBatches, certificateIdentifierBase64EndodedHash, signatureBase64EncodedHash);
            }
            return false;
        }

        private (string certificateIdentifier, string isoCode) GetCertificateIdentifierAndISOCodeFromTokenPayload(DCCPayload payload)
        {
            var obj = payload.DCCPayloadData.DCC;

            if (obj?.Vaccinations?.Any() ?? false)
                return (obj.Vaccinations.First().CountryOfVaccination, obj.Vaccinations.First().CertificateId);
            else if (obj?.Tests?.Any() ?? false)
                return (obj.Tests.First().CountryOfTest, obj.Tests.First().CertificateId);
            else if (obj?.Recovery?.Any() ?? false)
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

        private string Base64EncodeCertificateIdentifierHash(string certificateIdentifierHash) {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                var sha256Bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(certificateIdentifierHash));
                var sha256B64 = Convert.ToBase64String(sha256Bytes);

                var sha256Bytes2 = new byte[16];
                Array.Copy(sha256Bytes, sha256Bytes2, sha256Bytes2.Length);

                var certificateIdentifierBase64EndodedHash = Convert.ToBase64String(sha256Bytes2);
                return certificateIdentifierBase64EndodedHash;
            }

        }

        public bool CheckHashInRevocationBatchesAsync(IEnumerable<RevocationBatch> revocationBatches, string certificateIdentifierBase64EndodedHash, string signatureBase64EncodedHash, bool isParallel=false)
        {
                var result = MobileUtils.ContainsCertificateFilterMobile(certificateIdentifierBase64EndodedHash, signatureBase64EncodedHash, revocationBatches, _bloomFilterBuckets, isParallel);
                return result;

        }
   
    }
}
