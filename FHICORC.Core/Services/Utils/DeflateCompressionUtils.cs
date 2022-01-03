﻿using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Utils
{
    public static class DeflateCompression
    {
        public static async Task<byte[]> CompressAsync(string input)
        {
            // Remove BOM and ZERO WIDTH SPACE
            input = input.Trim(new char[] { '\uFEFF', '\u200B' });

            using (MemoryStream MemoryStream = new MemoryStream())
            {
                using (DeflateStream DeflateStream = new DeflateStream(MemoryStream, CompressionMode.Compress))
                {
                    // set encoderShouldEmitUTF8Identifier to false to not include the BOM
                    using (StreamWriter StreamWriter = new StreamWriter(DeflateStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)))
                    {
                        await StreamWriter.WriteAsync(input);
                    }
                }
                return MemoryStream.ToArray();
            }
        }

        public static async Task<string> UncompressAsync(byte[] input)
        {
            using (MemoryStream MemoryStream = new MemoryStream(input))
            {
                using (DeflateStream DeflateStream = new DeflateStream(MemoryStream, CompressionMode.Decompress))
                {
                    using (StreamReader StreamReader = new StreamReader(DeflateStream, Encoding.UTF8))
                    {
                        return await StreamReader.ReadToEndAsync();
                    }
                }
            }
        }
    }
}
