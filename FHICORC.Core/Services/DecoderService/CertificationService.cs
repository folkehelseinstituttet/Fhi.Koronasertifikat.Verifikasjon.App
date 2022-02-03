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
using Org.BouncyCastle.Asn1.Bsi;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

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

            // Get public key for jwk (Elliptic Curve)
            ECPublicKeyParameters jwkSignature = await GetJWKSignature(kid, issuer + "/.well-known/jwks.json");

            // Verify signature
            VerifySignature(jwkSignature, jws);
        }

        public async Task<SmartHealthCardIssuer> VerifySHCIssuer(JwsParts jws)
        {
            string payloadJson = await jws.DecodedPayload();
            string issuerString = GetJsonValue(payloadJson, "iss");
            SmartHealthCardIssuer issuer = await _smartHealthCardRepository.GetIssuerTrust(issuerString);

            Debug.Print($"Looked up SHC issuer {issuerString}, found name \"{issuer.Name}\", with trusted state: {issuer.Trusted}");
            if (!issuer.Trusted)
            {
                throw new Exception($"SHC issuer {issuerString} is not trusted");
            }
            return issuer;
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

        private async Task<ECPublicKeyParameters> GetJWKSignature(string kid, string url)
        {
            string responseJson = await _restClient.GetSimpleString(url);

            JsonWebKeySet jwks = new JsonWebKeySet(responseJson);
            List<JsonWebKey> keyList = new List<JsonWebKey>(jwks.Keys);
            JsonWebKey matchingKey = keyList.FirstOrDefault(x => x.Kid == kid);
            if (matchingKey == null)
            {
                throw new KeyNotFoundException($"{nameof(CertificationService)}.{nameof(GetJWKSignature)}: " +
                    $"No matching key for kid {kid} is found at URL {url}");
            }

            X9ECParameters parameters = ECNamedCurveTable.GetByOid(SecObjectIdentifiers.SecP256r1);
            ECPoint point = parameters.Curve.CreatePoint(
                new BigInteger(1, Base64UrlDecodingUtils.Base64UrlDecode(matchingKey.X)),
                new BigInteger(1, Base64UrlDecodingUtils.Base64UrlDecode(matchingKey.Y)));
            ECDomainParameters domainParameters = new ECDomainParameters(parameters.Curve, parameters.G, parameters.N, parameters.H, parameters.GetSeed());
            return new ECPublicKeyParameters(point, domainParameters);
        }

        private void VerifySignature(ECPublicKeyParameters jwkSignature, JwsParts jws)
        {
            byte[] MessageBytes = Encoding.UTF8.GetBytes(jws.EncodedPart(JwtPartsIndex.Header) + '.' + jws.EncodedPart(JwtPartsIndex.Payload));
            byte[] Signature = jws.DecodedSignature();

            ISigner signer = SignerUtilities.GetSigner(BsiObjectIdentifiers.ecdsa_plain_SHA256);
            signer.Init(false, jwkSignature);
            signer.BlockUpdate(MessageBytes, 0, MessageBytes.Length);
            bool isSignatureValid = signer.VerifySignature(Signature);

            Debug.Print($"{nameof(CertificationService)}.{nameof(VerifySHCSignature)}: JWS verification result - {(isSignatureValid ? "PASSED" : "FAILED")}");
            if (!isSignatureValid)
            {
                throw new InvalidSignatureException("Failed to verify JWS signature");
            }
        }
    }
}
