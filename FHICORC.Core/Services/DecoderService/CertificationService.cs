using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.CoseModel;
using FHICORC.Core.Services.Model.Exceptions;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;
using FHICORC.Core.Services.Utils;
using FHICORC.Core.WebServices;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace FHICORC.Core.Services.DecoderServices
{
    public class CertificationService: ICertificationService
    {
        private readonly IPublicKeyService _publicKeyService;
        private readonly IRestClient _restClient;
        private readonly ISmartHealthCardRepository _smartHealthCardRepository;

        public CertificationService(
            IPublicKeyService publicKeyService,
            IRestClient restClient,
            ISmartHealthCardRepository smartHealthCardRepository
        )
        {
            _publicKeyService = publicKeyService;
            _restClient = restClient;
            _smartHealthCardRepository = smartHealthCardRepository;
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

        public async Task VerifySHCSignature(JwsParts jws)
        {
            // Get json values from jws
            string headerJson = jws.DecodedHeader();
            string kid = GetJsonValue(headerJson, "kid");
            string payloadJson = await jws.DecodedPayload();
            string issuer = GetJsonValue(payloadJson, "iss");

            // Get public key
            string publicKeyString = await GetX5cForGivenKidAsync(kid, issuer + "/.well-known/jwks.json");
            byte[] publicKeyBase64BytesArray = Base64UrlDecodingUtils.Base64UrlDecode(publicKeyString);
            X509Certificate publicKeyX509Cert = new X509CertificateParser().ReadCertificate(publicKeyBase64BytesArray);

            // Verify signature
            byte[] MessageBytes = Encoding.UTF8.GetBytes(jws.EncodedPart(JwtPartsIndex.Header) + '.' + jws.EncodedPart(JwtPartsIndex.Payload));
            byte[] Signature = jws.DecodedSignature();
            var isSignatureValid = VerifySignature(publicKeyX509Cert, Signature, MessageBytes);
            Debug.Print($"{nameof(CertificationService)}.{nameof(VerifySHCSignature)}: JWS verification result - {(isSignatureValid ? "PASSED" : "FAILED")}");
            if (!isSignatureValid)
            {
                throw new Exception("Failed to verify JWS signature");
            }
        }

        public async Task VerifySHCIssuer(JwsParts jws)
        {
            string payloadJson = await jws.DecodedPayload();
            string issuerString = GetJsonValue(payloadJson, "iss");
            SmartHealthCardIssuer issuer = await _smartHealthCardRepository.GetIssuerTrust(issuerString);

            Debug.Print($"Looked up SHC issuer {issuerString}, found name \"{issuer.Name}\", with trusted state: {issuer.Trusted}");
            if (!issuer.Trusted)
            {
                throw new Exception($"SHC issuer {issuerString} is not trusted");
            }
        }

        private string GetJsonValue(string json, string jsonKey)
        {
            JObject decodedData = JObject.Parse(json);
            string value = (string)decodedData.GetValue(jsonKey) ?? null;
            if (string.IsNullOrEmpty(value))
            {
                throw new MissingDataException($"{jsonKey} is missing");
            }
            return value;
        }

        private async Task<string> GetX5cForGivenKidAsync(string kid, string url)
        {
            string responseJson = await _restClient.GetSimpleString(url);

            JsonWebKeySet jwks = new JsonWebKeySet(responseJson);
            List<JsonWebKey> keyList = new List<JsonWebKey>(jwks.Keys);

            JsonWebKey matchingKey = keyList.FirstOrDefault(x => x.Kid == kid);

            if (matchingKey == null)
            {
                throw new KeyNotFoundException($"{nameof(CertificationService)}.{nameof(GetX5cForGivenKidAsync)}: " +
                    $"No matching key for kid {kid} is found at URL {url}");
            }
            if (matchingKey.X5c.IsNullOrEmpty())
            {
                throw new MissingDataException($"{nameof(CertificationService)}.{nameof(GetX5cForGivenKidAsync)}: " +
                    $"No X.509 public keys found for kid {kid} at URL {url}");
            }

            return matchingKey.X5c[0];
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