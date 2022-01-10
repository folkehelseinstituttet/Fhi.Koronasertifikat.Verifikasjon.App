using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.CoseModel;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;
using FHICORC.Core.Services.Utils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;

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

        public async Task VerifySHCSignature(JwsParts jws, string shcJson)
        {
            Debug.Print($"{nameof(CertificationService)}.{nameof(VerifySHCSignature)}: Decoded JWS Payload - {shcJson}");
            JObject payloadData = JObject.Parse(shcJson);
            string iss = (string)payloadData.GetValue("iss") ?? null;
            if (string.IsNullOrEmpty(iss))
            {
                throw new InvalidDataException("iss is missing");
            }

            byte[] DecodedHeader = Base64UrlDecodingUtils.Base64UrlDecode(jws.Header);
            string HeaderJson = Encoding.UTF8.GetString(DecodedHeader);
            Debug.Print($"{nameof(CertificationService)}.{nameof(VerifySHCSignature)}: Decoded JWS Header - {HeaderJson}");
            JObject headerData = JObject.Parse(HeaderJson);
            string kid = (string)headerData.GetValue("kid") ?? null;
            if (string.IsNullOrEmpty(kid))
            {
                throw new InvalidDataException("kid is missing");
            }

            JsonWebKey jwk = await GetX5cForGivenKidAsync(kid, iss + "/.well-known/jwks.json");
            string publicKeyString = jwk.X5c[0];
            byte[] publicKeyBase64BytesArray = Base64UrlDecodingUtils.Base64UrlDecode(publicKeyString);
            X509Certificate publicKeyX509Cert = new X509CertificateParser().ReadCertificate(publicKeyBase64BytesArray);

            byte[] MessageBytes = Encoding.UTF8.GetBytes(jws.Header + '.' + jws.Payload);
            byte[] Signature = Base64UrlDecodingUtils.Base64UrlDecode(jws.Signature);

            var isSignatureValid = VerifySignature(publicKeyX509Cert, Signature, MessageBytes);
            Debug.Print($"{nameof(CertificationService)}.{nameof(VerifySHCSignature)}: JWS verification result - {(isSignatureValid ? "PASSED" : "FAILED")}");
            if (!isSignatureValid)
            {
                throw new Exception("Failed to verify JWS signature");
            }
        }

        private async Task<JsonWebKey> GetX5cForGivenKidAsync(string kid, string url)
        {
            HttpClient _client = new HttpClient();
            string responseJson = await _client.GetStringAsync(url);

            JsonWebKeySet jwks = new JsonWebKeySet(responseJson);
            List<JsonWebKey> keyList = new List<JsonWebKey>(jwks.Keys);

            JsonWebKey matchingKey = keyList.FirstOrDefault(x => x.Kid == kid);

            if (matchingKey == null)
            {
                throw new Exception($"{nameof(CertificationService)}.{nameof(GetX5cForGivenKidAsync)}: " +
                    $"No matching key for kid {kid} is found at URL {url}");
            }
            return matchingKey;
        }

        private bool VerifySignature(X509Certificate pubKeyCert, byte[] sigBytes, byte[] msgBytes)
        {
            try
            {
                var publicKey = pubKeyCert.GetPublicKey();
                var signer = SignerUtilities.GetSigner("SHA-256withPLAIN-ECDSA");

                signer.Init(false, publicKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Verification failed with the error: " + exc.ToString());
                return false;
            }
        }
    }
}