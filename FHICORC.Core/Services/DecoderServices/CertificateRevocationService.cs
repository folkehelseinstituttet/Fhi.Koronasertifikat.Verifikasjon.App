using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Interface;
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
            { "BE", (x) => x.Split('#')[0] },                   // "01CountryOpaqueUniqueString#Checksum"                                   Example: "01BEVLMP7BEIN6GQWZTAKP6OBBAN#J"
            { "CY", (x) => x.Split('#')[0].Split(':')[2] },     // "dgci:V1:Country:OpaqueUniqueString#Checksum"                            Example: "dgci:V1:CY:GRHC8O6ECPWMUM16D3WAXOPN4:89"
            { "DE", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01Country/IssuingEntity/OpaqueUniqueString#Checksum"           Example: "URN:UVCI:01DE/IZ12345A/5CWLU12RNOB9RXSEOP6FG8#W",
            { "ES", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "01ES07T895AA7BF69D99C77DB96B#E"
            { "FR", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "dgci:V1:FR:C51AOQW7CQMFW7WLIWVGADQY6:70"
            { "IT", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "01ITE7300E1AB2A84C719004F103DCB1F70A#6"
            { "LU", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "01/LU/1GJBHZ9HOOZ8W#3K",
            { "PT", (x) => x.Split('#')[0].Split(':')[2] },     // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "urn:uvci:01/PT/SPMS/TRC01234567890123456#1",
            { "EE", (x) => x.Split('#')[0].Split(':')[2] }      // "URN:UVCI:01:Country:OpaqueUniqueString#Checksum"                        Example: "01/EE/TIS/9C0KAVIIYH7CZAB02O#O"
        };

        private readonly IRevocationBatchService _revocationBatchService;

        public CertificateRevocationService(IRevocationBatchService revocationBatchService) => _revocationBatchService = revocationBatchService;

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
            int m = 0;
            int k = 0;

            foreach (var batch in revocationBatches)
            {
                // If any hash functions hits a bit with a zero, go to next batch (contiune)


                if (batch.Bits.Contains(certificateIdentifierHash, m, k))
                    return true;
            }

            return false;
        }
    }
}
