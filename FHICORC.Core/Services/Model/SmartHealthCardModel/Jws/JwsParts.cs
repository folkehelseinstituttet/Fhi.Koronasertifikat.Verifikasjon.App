using System;
using System.Threading.Tasks;
using FHICORC.Core.Services.Utils;
using System.Diagnostics;
using System.Text;
using FHICORC.Core.Services.Model.Exceptions;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Jws
{
    public class JwsParts
    {
        public JwsParts(string[] Parts)
        {
            if (Parts is null)
                throw new ArgumentNullException(nameof(Parts));
            if (Parts.Length != 3)
                throw new Exception($"{nameof(Parts)}: A JWS Token must have three parts separated by dots (e.g Header.Payload.Signature).");
            this.EncodedParts = Parts;
        }

        public static JwsParts ParseToken(string Token)
        {
            if (string.IsNullOrEmpty(Token))
                throw new MissingDataException($"{nameof(EncodedParts)}: A JWS Token must not be null.");

            var parts = Token.Split('.');
            if (parts.Length != 3)
                throw new Exception($"{nameof(EncodedParts)}: A JWS Token must have three parts separated by dots (e.g Header.Payload.Signature).");

            return new JwsParts(parts);
        }

        public string EncodedPart(JwtPartsIndex part)
        {
            return this.EncodedParts[(int)part];
        }

        public string DecodedHeader()
        {
            if (_DecodedHeader == null)
            {
                byte[] decodedPart = Base64UrlDecodingUtils.Base64UrlDecode(EncodedPart(JwtPartsIndex.Header));
                string decodedString = Encoding.UTF8.GetString(decodedPart);
                Debug.Print($"{nameof(JwsParts)}: Decoded JWS {JwtPartsIndex.Header} - {decodedString}");
                _DecodedHeader = decodedString;
            }
            return _DecodedHeader;
        }

        public async Task<string> DecodedPayload()
        {
            if (_DecodedPayload == null)
            {
                byte[] decodedPart = Base64UrlDecodingUtils.Base64UrlDecode(EncodedPart(JwtPartsIndex.Payload));
                string decodedString = await DeflateCompression.UncompressAsync(decodedPart);
                Debug.Print($"{nameof(JwsParts)}: Decoded JWS {JwtPartsIndex.Payload} - {decodedString}");
                _DecodedPayload = decodedString;
            }
            return _DecodedPayload;
        }


        public byte[] DecodedSignature()
        {
            if (_DecodedSignature == null)
            {
                byte[] decodedPart = Base64UrlDecodingUtils.Base64UrlDecode(EncodedPart(JwtPartsIndex.Signature));
                Debug.Print($"{nameof(JwsParts)}: Decoded JWS {JwtPartsIndex.Signature} - {decodedPart}");
                _DecodedSignature = decodedPart;
            }
            return _DecodedSignature;
        }

        // Parts is base64 encoded strings.
        private string[] EncodedParts { get; set; }

        // Local cache, to not decode for each call and only decode when needed.
        private string _DecodedHeader;
        private string _DecodedPayload;
        private byte[] _DecodedSignature;
    }

    public enum JwtPartsIndex
    {
        Header = 0,
        Payload = 1,
        Signature = 2
    }
}