using System;
using System.Collections.Generic;
using System.Diagnostics;
using Org.BouncyCastle.Security;
using PeterO.Cbor;
using FHICORC.Core.Services.Utils;

namespace FHICORC.Core.Services.Model.CoseModel
{
    public class CoseSign1Object
    {
        /** The COSE_Sign1 message tag. */
        public static int MESSAGE_TAG = 18;

        /** Should the message tag be included? The default is {@code false}. */
        private bool includeMessageTag = false;

        /** The protected attributes. */
        private CBORObject protectedAttributes;

        /** The encoding of the protected attributes. */
        private byte[] protectedAttributesEncoding;

        /** The unprotected attributes. */
        private CBORObject unprotectedAttributes;

        /** The data content (data that is signed). */
        private byte[] content;

        /** The signature. */
        private byte[] signature;

        /** We don't support external data - so it's static. */
        private static byte[] externalData = new byte[0];

        /** The COSE_Sign1 context string. */
        private static String contextString = "Signature1";

        /**
         * Default constructor.
         */
        public CoseSign1Object()
        {
            this.protectedAttributes = CBORObject.NewMap();
            this.unprotectedAttributes = CBORObject.NewMap();
        }

        /**
         * Constructor that accepts the binary representation of a signed COSE_Sign1 object.
         * 
         * @param data
         *          the binary representation of the COSE_Sign1 object
         * @throws CBORException
         *           for invalid data
         */
        public CoseSign1Object(byte[] data)
        {
            CBORObject message = CBORObject.DecodeFromBytes(data);
            if (message.Type != CBORType.Array)
            {
                throw new CBORException("Supplied message is not a valid COSE security object");
            }

            // If the message is tagged, it must have the message tag for a COSE_Sign1 message.
            //
            if (message.IsTagged) {
                if (message.GetAllTags().Length != 1) {
                    throw new CBORException("Invalid object - too many tags");
                }
                if (MESSAGE_TAG != message.MostInnerTag.ToInt32Unchecked())
                {
                    throw new CBORException(String.Format(
                      "Invalid COSE_Sign1 structure - Expected {0} tag - but was {1}",
                      MESSAGE_TAG, message.MostInnerTag.ToInt32Unchecked()));
                }
            }

            if (message.Count != 4)
            {
                throw new CBORException(String.Format(
                  "Invalid COSE_Sign1 structure - Expected an array of 4 items - but array has {0} items", message.Count));
            }
            if (message[0].Type == CBORType.ByteString)
            {
                this.protectedAttributesEncoding = message[0].GetByteString();

                if (message[0].GetByteString().Length == 0)
                {
                    this.protectedAttributes = CBORObject.NewMap();
                }
                else
                {
                    this.protectedAttributes = CBORObject.DecodeFromBytes(this.protectedAttributesEncoding);
                    if (this.protectedAttributes.Count == 0)
                    {
                        this.protectedAttributesEncoding = new byte[0];
                    }
                }
            }
            else
            {
                throw new CBORException(String.Format("Invalid COSE_Sign1 structure - " +
                    "Expected item at position 1/4 to be a bstr which is the encoding of the protected attributes, but was {0}",
                  message[0].Type));
            }

            if (message[1].Type == CBORType.Map)
            {
                this.unprotectedAttributes = message[1];
            }
            else
            {
                throw new CBORException(String.Format(
                  "Invalid COSE_Sign1 structure - Expected item at position 2/4 to be a Map for unprotected attributes, but was {0}",
                  message[1].Type));
            }

            if (message[2].Type == CBORType.ByteString)
            {
                this.content = message[2].GetByteString();
            }
            else if (!message[2].IsNull)
            {
                throw new CBORException(String.Format(
                  "Invalid COSE_Sign1 structure - Expected item at position 3/4 to be a bstr holding the payload, but was {0}",
                  message[2].Type));
            }

            if (message[3].Type == CBORType.ByteString)
            {
                this.signature = message[3].GetByteString();
            }
            else
            {
                throw new CBORException(String.Format(
                  "Invalid COSE_Sign1 structure - Expected item at position 4/4 to be a bstr holding the signature, but was {0}",
                  message[3].Type));
            }
        }
        public static CoseSign1Object Decode(byte[] data)
        {
            return new CoseSign1Object(data);
        }
        /**
         * A utility method that looks for the key identifier (kid) in the protected (and unprotected) attributes.
         * 
         * @return the key identifier as a byte string
         */
        public byte[] GetKeyIdentifier()
        {
            CBORObject kid = this.protectedAttributes[HeaderParameterKey.KID];
            if( kid == null)
            {
                kid = this.unprotectedAttributes[HeaderParameterKey.KID];
            }

            if (kid == null)
            {
                return null;
            }
            return kid.GetByteString();
        }
        
        /**
         * A utility method that gets the contents as a {@link Cwt}.
         * 
         * @return the CWT or null if no contents is available
         * @throws CBORException
         *           if the contents do not hold a valid CWT
         */
        public string GetJson()
        {
            if (this.content == null) {
                return null;
            }
            return CborUtils.ToJson(content);
        }

        /**
         * Verifies the signature of the COSE_Sign1 object.
         * <p>
         * Note: This method only verifies the signature. Not the payload.
         * </p>
         * 
         * @param publicKey
         *          the key to use when verifying the signature
         * @throws SignatureException
         *           for signature verification errors
         */
        public void VerifySignature(byte[] publicKey)
        {
            if (this.signature == null) {
                throw new Exception("Object is not signed");
            }

            CBORObject obj = CBORObject.NewArray();
            obj.Add(contextString);
            obj.Add(this.protectedAttributesEncoding);
            obj.Add(externalData);
            if (this.content != null) {
                obj.Add(this.content);
            }
            else {
                obj.Add(null);
            }

            byte[] signedData = obj.EncodeToBytes();

            // First find out which algorithm to use by searching for the algorithm ID in the protected attributes.
            //
            CBORObject registeredAlgorithm = this.protectedAttributes[HeaderParameterKey.ALG];
            if (registeredAlgorithm == null)
            {
                throw new Exception("No algorithm ID stored in protected attributes - cannot sign");
            }
            
            byte[] signatureToVerify = this.signature;
            if (!SignatureAlgorithm.IsSupportedAlgorithm(registeredAlgorithm))
            {
                throw new NotSupportedException(
                    $"token are signed with unsupported algorithm {SignatureAlgorithm.GetAlgorithmName(registeredAlgorithm)}");
            }

            // For ECDSA, convert the signature according to section 8.1 of RFC8152.
            if (registeredAlgorithm == SignatureAlgorithm.ES256
                || registeredAlgorithm == SignatureAlgorithm.ES384
                || registeredAlgorithm == SignatureAlgorithm.ES512)
            {

                signatureToVerify = ConvertToDer(this.signature);
            }


            // Verify using the public key
            var pubkey = PublicKeyFactory.CreateKey(publicKey);

            var verifier = SignerUtilities.GetSigner(SignatureAlgorithm.GetAlgorithmName(registeredAlgorithm));
            verifier.Init(false, pubkey);
            verifier.BlockUpdate(signedData, 0, signedData.Length);
            var result = verifier.VerifySignature(signatureToVerify);

            if (!result)
            {
                throw new Exception("Signature did not verify correctly");
            }
            Debug.WriteLine("result: " + result);
        }

        /**
         * Given a signature according to section 8.1 in RFC8152 its corresponding DER encoding is returned.
         * 
         * @param rsConcat
         *          the ECDSA signature
         * @return DER-encoded signature
         */
        private static byte[] ConvertToDer(byte[] rsConcat)
        {
            int len = rsConcat.Length / 2;
            byte[] r = new byte[len];
            byte[] s = new byte[len];
            Array.Copy(rsConcat, r, len);
            Array.Copy(rsConcat, len, s, 0, len);

            List<byte[]> seq = new List<byte[]>();
            seq.Add(Asn1Utils.ToUnsignedInteger(r));
            seq.Add(Asn1Utils.ToUnsignedInteger(s));

            return Asn1Utils.ToSequence(seq);
        }

    }
}