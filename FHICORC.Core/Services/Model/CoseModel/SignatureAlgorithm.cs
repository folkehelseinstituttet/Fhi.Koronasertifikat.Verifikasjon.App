using System;
using System.Linq;
using PeterO.Cbor;

namespace FHICORC.Core.Services.Model.CoseModel
{
    public class SignatureAlgorithm
    {
        /** ECDSA with SHA-256. */
        public static readonly CBORObject ES256 = CBORObject.FromObject(-7);

        /** ECDSA with SHA-384. */
        public static readonly CBORObject ES384 = CBORObject.FromObject(-35);

        /** ECDSA with SHA-512. */
        public static readonly CBORObject ES512 = CBORObject.FromObject(-36);

        /** RSASSA-PSS with SHA-256. */
        public static readonly CBORObject PS256 = CBORObject.FromObject(-37);

        /** RSASSA-PSS with SHA-384. */
        public static readonly CBORObject PS384 = CBORObject.FromObject(-38);

        /** RSASSA-PSS with SHA-512. */
        public static readonly CBORObject PS512 = CBORObject.FromObject(-39);


        public static String GetAlgorithmName(CBORObject cborValue)
        {
            switch (cborValue.AsInt32())
            {
                case -7:
                    return "SHA256withECDSA";
                case -35:
                    return "SHA384withECDSA";
                case -36:
                    return "SHA512withECDSA";
                case -37:
                    return "SHA256withRSA/PSS";
                case -38:
                    return "SHA384withRSA/PSS";
                case -39:
                    return "SHA512withRSA/PSS";
                default:
                    break;
            }
            return null;
        }

        private static CBORObject[] SupportedAlgorithm = new[]
        {
            ES256,
            PS256
        };
        public static bool IsSupportedAlgorithm(CBORObject cborValue)
        {
            return SupportedAlgorithm.Contains(cborValue);
        }
    }
}