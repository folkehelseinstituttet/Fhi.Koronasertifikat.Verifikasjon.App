using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            Console.WriteLine($"SmartHealthCardJson: {shcJson}");
            JObject payloadData = JObject.Parse(shcJson);
            string iss = (string)payloadData.GetValue("iss") ?? null;
            if (string.IsNullOrEmpty(iss))
            {
                throw new InvalidDataException("iss is missing");
            }

            byte[] DecodedHeader = Base64UrlDecodingUtils.Base64UrlDecode(jws.Header);
            string HeaderJson = Encoding.UTF8.GetString(DecodedHeader);
            Console.WriteLine($"HeaderJson: {HeaderJson}");
            JObject headerData = JObject.Parse(HeaderJson);
            string kid = (string)headerData.GetValue("kid") ?? null;

            if (string.IsNullOrEmpty(kid))
            {
                throw new InvalidDataException("kid is missing");
            }

            var key = await GetX5cForGivenKidAsync(kid, iss + "/.well-known/jwks.json");
            var pubKeyString = key.X5c[0];
            var pubKeyBytes = HexStringToByteArray(pubKeyString);

            //BigInteger xbi = new BigInteger(pubkeybytes.Take(32).ToArray());
            //BigInteger ybi = new BigInteger(pubkeybytes.Skip(32).ToArray());

            X9ECParameters curve = SecNamedCurves.GetByName("secp128r1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var q = curve.Curve.DecodePoint(Hex.Decode(pubKeyBytes));
            // Alternatively, try creating Q from X/Y
            //var xbi = new BigInteger(Base64UrlDecode(key.X));
            //var ybi = new BigInteger(Base64UrlDecode(key.Y));
            //curve.Curve.CreatePoint(xbi, ybi);

            ECPublicKeyParameters publicKey = new ECPublicKeyParameters("ECDSA", q, domain);

            byte[] BytesToSign = Utf8EncodingSupport.GetBytes(jws.Header, (byte)'.', jws.Payload);
            byte[] Signature = Base64UrlDecodingUtils.Base64UrlDecode(jws.Signature);

            var isSignatureValid = VerifySignature(publicKey, Signature, BytesToSign);

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
                return null;
            }

            return matchingKey;
        }

        private bool VerifySignature(ECPublicKeyParameters pubKey, byte[] sigBytes, byte[] msgBytes)
        {
            try
            {
                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                //var signer = SignerUtilities.GetSigner("SHA-256withPLAIN-ECDSA");

                signer.Init(false, pubKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Verification failed with the error: " + exc.ToString());
                return false;
            }
        }

        public static byte[] HexStringToByteArray(string Hex)
        {
            byte[] Bytes = new byte[Hex.Length / 2];
            int[] HexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
                                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D,
                                 0x0E, 0x0F };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
            {
                Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                  HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
            }

            return Bytes;
        }
    }
}