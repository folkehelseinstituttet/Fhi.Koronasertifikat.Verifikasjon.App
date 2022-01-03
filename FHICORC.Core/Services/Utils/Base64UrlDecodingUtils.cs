using System;
namespace FHICORC.Core.Services.Utils
{
    public class Base64UrlDecodingUtils
    {
        // Src: https://jonlabelle.com/snippets/view/csharp/base64-url-encode-and-decode
        public static byte[] Base64UrlDecode(string input)
        {
            var output = input;

            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 1:
                    output += "==="; break; // Three pad chars
                case 2:
                    output += "=="; break; // Two pad chars
                case 3:
                    output += "="; break; // One pad char
                default:
                    throw new System.Exception("Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output); // Standard base64 decoder

            return converted;
        }
    }
}
