using PeterO.Cbor;

namespace FHICORC.Core.Services.Model.CoseModel
{
    //https://tools.ietf.org/html/rfc8152#section-3
    public class HeaderParameterKey
    {

        /** Algorithm used for security processing. */
        public static readonly CBORObject ALG = CBORObject.FromObject(1);

        /** Critical headers to be understood. */
        public static readonly CBORObject CRIT = CBORObject.FromObject(2);

        /** This parameter is used to indicate the content type of the data in the payload or ciphertext fields. */
        public static readonly CBORObject CONTENT_TYPE = CBORObject.FromObject(3);

        /** This parameter identifies one piece of data that can be used as input to find the needed cryptographic key. */
        public static readonly CBORObject KID = CBORObject.FromObject(4);
    }
}