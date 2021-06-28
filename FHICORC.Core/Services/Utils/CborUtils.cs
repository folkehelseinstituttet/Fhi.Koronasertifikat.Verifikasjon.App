using PeterO.Cbor;

namespace FHICORC.Core.Services.Utils
{
    public static class CborUtils
    {
        public static string ToJson(byte[] cborDataFormatBytes)
        {
            // Convert from bytes to CBORObject then to jsonString
            CBORObject cborObjectFromBytes = CBORObject.DecodeFromBytes(cborDataFormatBytes);
            string jsonString = cborObjectFromBytes.ToJSONString();

            return jsonString;
        }
        // public static byte[] JsonToCbor(string jsonString)
        // {
        //     //Convert from jsonString to CBORObject then to bytes
        //     CBORObject cborFormatedJson = CBORObject.FromJSONString(jsonString);
        //     byte[] cborDataFormatBytes = cborFormatedJson.EncodeToBytes();
        //
        //     return cborDataFormatBytes;
        //
        // }

        

        // public static RSAParameters CreateKey()
        // {
        //     // Create a new key pair
        //     RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        //     RSAParameters Key = RSAalg.ExportParameters(true);
        //     return Key;
        // }

        // public static byte[] SignCborFile(byte[] cborBytesToSign, RSAParameters key)
        // {
        //     try
        //     {
        //         // Hash and sign the data.
        //         byte[] signedData = HashAndSignBytes(cborBytesToSign, key);
        //         if (VerifySignedHash(cborBytesToSign, signedData, key))
        //         {
        //             Console.WriteLine("The data was signed and verified correctly.");
        //         }
        //         else
        //         {
        //             Console.WriteLine("The data does not match the signature, not signed correctly.");
        //         }
        //         return signedData;
        //     }
        //     catch (ArgumentNullException)
        //     {
        //         Console.WriteLine("The data was not signed or verified");
        //         return null;
        //     }
        // }

        // public static byte[] HashAndSignBytes(byte[] dataToSign, RSAParameters key)
        // {
        //     try
        //     {
        //         // Create a new instance of RSACryptoServiceProvider using the
        //         // key from RSAParameters.
        //         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        //
        //         RSAalg.ImportParameters(key);
        //
        //         // Hash and sign the data. Pass a new instance of SHA256
        //         // to specify the hashing algorithm.
        //         return RSAalg.SignData(dataToSign, SHA256.Create());
        //     }
        //     catch (CryptographicException e)
        //     {
        //         Console.WriteLine(e.Message);
        //
        //         return null;
        //     }
        // }

        // public static bool VerifySignedHash(byte[] dataToVerify, byte[] signedData, RSAParameters key)
        // {
        //     try
        //     {
        //         RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        //
        //         RSAalg.ImportParameters(key);
        //
        //         return RSAalg.VerifyData(dataToVerify, SHA256.Create(), signedData);
        //     }
        //     catch (CryptographicException e)
        //     {
        //         Console.WriteLine(e.Message);
        //
        //         return false;
        //     }
        // }
        //
        // public static byte[] AddMetaDataToCbor(byte[] originalCborBytes, byte[] signedCborBytes, RSAParameters key)
        // {
        //     string signedCborString = Convert.ToBase64String(signedCborBytes);
        //     CBORObject originalCborObject = CBORObject.DecodeFromBytes(originalCborBytes);
        //
        //     //// More metadata can be added here, if requested
        //     //RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        //
        //     //RSAalg.ImportParameters(key);
        //     //Console.WriteLine(RSAalg.ExportRSAPublicKey());
        //     originalCborObject.Add("Alg", "SHA-256");
        //     originalCborObject.Add("Signature", signedCborString);
        //
        //     // Convert altered cbor back to bytes
        //     byte[] alteredCborBytes = originalCborObject.EncodeToBytes();
        //
        //     Console.WriteLine(originalCborObject);
        //
        //     return alteredCborBytes;
        //
        // }
        //
        // public static bool VerifyFinalData(dynamic parsedJsonObject, string originalJsonString, RSAParameters key)
        // {
        //     // Takes new unpacked/uncompressed data and original data and compares to check if no data has been lost etc. 
        //     dynamic originalParsedJsonString = JObject.Parse(originalJsonString);
        //
        //     string parsedOriginalJsonStringBody = originalParsedJsonString.Body.ToString();
        //     string parsedUnpackedJsonStringBody = parsedJsonObject.Body.ToString();
        //
        //     // The Equals method comparing both strings isnt really neccesary as the VerifySignature method already does this.
        //     // Also original originalJsonString isnt needed and wont be accessible in real situation, the data can be obtained from the unpacked
        //     // data, which can then be verified with the included signature to ensure its the same as original.
        //     if (parsedUnpackedJsonStringBody.Equals(parsedOriginalJsonStringBody) && VerifySignature(parsedUnpackedJsonStringBody, parsedJsonObject, key))
        //     {
        //         return true;
        //
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        // private static bool VerifySignature(string jsonStringToTest, dynamic parsedJsonObject, RSAParameters key)
        // {
        //     byte[] newCborObject = JsonToCbor(jsonStringToTest);
        //     // Verify signature
        //     string signatureString = parsedJsonObject.Signature;
        //     byte[] signatureBytes = Convert.FromBase64String(signatureString);
        //     // Check if signature is correct after decompression, compared to original bytes
        //     if (VerifySignedHash(newCborObject, signatureBytes, key))
        //     {
        //         return true;
        //     }
        //     return false;
        // }
    }
}
