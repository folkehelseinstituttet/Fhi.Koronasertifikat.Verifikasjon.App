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

namespace FHICORC.Core.Services.DecoderServices
{
    public class CertificateRevocationService : ICertificateRevocationService
    {
        private readonly IRevocationBatchService _revocationBatchService;
        private readonly List<BucketItem> _bloomFilterBuckets;

        public CertificateRevocationService(IRevocationBatchService revocationBatchService)
        {
            _revocationBatchService = revocationBatchService;
            _bloomFilterBuckets = MobileUtils.FillBloomBuckets();
        }


        public async Task<bool> IsCertificateRevoked(ITokenPayload token, string signatureBase64EncodedHash)
        {
            if (token is DCCPayload payload)
            {
                (var isoCode, var certificateIdentifierWithChecksum) = GetCertificateIdentifierAndISOCodeFromTokenPayload(payload);
                var certificateIdentifier = RemoveCheckSum(certificateIdentifierWithChecksum);
                var revocationBatches = await _revocationBatchService.GetRevocationBatchesFromCountry(isoCode);

                var uci = Base64EncodeIdentifier(certificateIdentifier);
                var ccUci = Base64EncodeIdentifier(isoCode + certificateIdentifier);

                return CheckHashInRevocationBatchesAsync(revocationBatches, uci, ccUci, signatureBase64EncodedHash);
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

        private string RemoveCheckSum(string certificateIdentifier)
        {
            var certificateIdentifierWithoutCheckSum = certificateIdentifier.Split('#').First();
            return certificateIdentifierWithoutCheckSum;
        }

        private string Base64EncodeIdentifier(string certificateIdentifierHash) {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                var shaBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(certificateIdentifierHash));

                var shaBytes16 = new byte[16];
                Array.Copy(shaBytes, shaBytes16, shaBytes16.Length);


#if DEBUG
                //Show the sha256 hashed string
                var str = BytesToString(shaBytes16);
# endif
                var certificateIdentifierBase64EndodedHash = Convert.ToBase64String(shaBytes16);
                return certificateIdentifierBase64EndodedHash;
            }
        }

        public static string BytesToString(byte[] bytes) {
            StringBuilder Sb = new StringBuilder();

            foreach (Byte b in bytes)
                Sb.Append(b.ToString("x2"));

            return Sb.ToString();
        }


        public bool CheckHashInRevocationBatchesAsync(IEnumerable<RevocationBatch> revocationBatches, string uciBase64EndodedHash, string countryCodeUciBase64EndodedHash, string signatureBase64EncodedHash, bool isParallel = false)
        {
                var result = MobileUtils.ContainsCertificateFilterMobile(uciBase64EndodedHash, countryCodeUciBase64EndodedHash, signatureBase64EncodedHash, revocationBatches, _bloomFilterBuckets, isParallel);
                return result;

        }
   
    }
}
