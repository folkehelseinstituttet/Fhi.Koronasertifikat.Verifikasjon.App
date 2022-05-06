using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.CoseModel;

namespace FHICORC.Core.Services.DecoderServices
{
    public class CertificationService: ICertificationService
    {
        private readonly IPublicKeyService _publicKeyService;

        public CertificationService(IPublicKeyService publicKeyService)
        {
            _publicKeyService = publicKeyService;
        }
        public async Task VerifyCoseSign1Object(CoseSign1Object coseSign1Object)
        {
            string kidBase64 = Convert.ToBase64String(coseSign1Object.GetKeyIdentifier());

            List<string> publicKeys = await _publicKeyService.GetPublicKeyByKid(kidBase64);
            if (!publicKeys.Any())
            {
                throw new InvalidDataException(
                    $"no public key corespondent to provided key identifier found. key identifier: {kidBase64}");
            }

            foreach (string publicKey in publicKeys)
            {
                try
                {
                    byte[] pk = Convert.FromBase64String(publicKey);
                    coseSign1Object.VerifySignature(pk);
                    return;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            throw new Exception("Signature verification failed for all attempted keys");
        }
    }
}