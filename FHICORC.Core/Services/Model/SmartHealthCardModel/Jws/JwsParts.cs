using System;
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
            this.Parts = Parts;
        }

        public static JwsParts ParseToken(string Token)
        {
            if (string.IsNullOrEmpty(Token))
                throw new Exception($"{nameof(Parts)}: A JWS Token must not be null.");

            var parts = Token.Split('.');
            if (parts.Length != 3)
                throw new Exception($"{nameof(Parts)}: A JWS Token must have three parts separated by dots (e.g Header.Payload.Signature).");

            return new JwsParts(parts);
        }

        public string Header => this.Parts[(int)JwtPartsIndex.Header];

        public string Payload => this.Parts[(int)JwtPartsIndex.Payload];

        public string Signature => this.Parts[(int)JwtPartsIndex.Signature];

        public string[] Parts { get; private set; }
    }

    public enum JwtPartsIndex
    {
        Header = 0,
        Payload = 1,
        Signature = 2
    }
}