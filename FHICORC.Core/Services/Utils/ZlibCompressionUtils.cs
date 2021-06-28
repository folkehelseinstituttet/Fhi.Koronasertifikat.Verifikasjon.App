using System.IO;
using Ionic.Zlib;

namespace FHICORC.Core.Services.Utils
{
    public static class ZlibCompressionUtils
    {
        public static byte[] Decompress(byte[] compressedBytes)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZlibStream outZStream = new ZlibStream(outMemoryStream, CompressionMode.Decompress))
            using (Stream inMemoryStream = new MemoryStream(compressedBytes))
            {
                CopyStream(inMemoryStream, outZStream);
                return outMemoryStream.ToArray();
            }
        }
        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }  
    }
}