using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace FHICORC.Core.Services.Utils
{
    public static class BloomFilterUtils
    {
        //public static bool Contains(this BitArray filter, string str, int m, int k)
        //{
        //    var hash = HashData(Encoding.UTF8.GetBytes(str), m, k);

        //    foreach (int i in hash)
        //    {
        //        if (!filter[i])
        //            return false;
        //    }
        //    return true;
        //}

        //public static bool Contains(this BitArray filter, int[] hashData)
        //{
        //    foreach (int i in hashData)
        //    {
        //        var value = filter[i];
        //        if (!value)
        //            return false;
        //    }
        //    return true;
        //}

        public static int[] HashData(byte[] data, int m, int k)
        {
            var hashAlgorithm = SHA256.Create();

            int[] array = new int[k];
            int num = 0;
            byte[] array2 = new byte[0];
            byte[] outputBuffer = new byte[hashAlgorithm.HashSize / 8];

            while (num < k)
            {
                hashAlgorithm.TransformBlock(array2, 0, array2.Length, outputBuffer, 0);
                array2 = hashAlgorithm.ComputeHash(data, 0, data.Length);
                BitArray bit = new BitArray(array2);
                int num2 = (int)(32 - NumberOfLeadingZeros((uint)m));
                int num3 = array2.Length * 8;
                for (int i = 0; i < num3 / num2; i++)
                {
                    if (num >= k)
                    {
                        break;
                    }

                    int from = i * num2;
                    int to = (i + 1) * num2;
                    int num4 = BitToIntOne(bit, from, to);
                    if (num4 < m)
                    {
                        array[num] = num4;
                        num++;
                    }
                }
            }

            return array;
        }

        public static uint NumberOfLeadingZeros(uint i)
        {
            if (i == 0)
            {
                return 32u;
            }

            uint num = 1u;
            if (i >> 16 == 0)
            {
                num += 16;
                i <<= 16;
            }

            if (i >> 24 == 0)
            {
                num += 8;
                i <<= 8;
            }

            if (i >> 28 == 0)
            {
                num += 4;
                i <<= 4;
            }

            if (i >> 30 == 0)
            {
                num += 2;
                i <<= 2;
            }

            return num - (i >> 31);
        }

        public static int BitToIntOne(BitArray bit, int from, int to)
        {
            int num = to - from;
            int count = bit.Count;
            int num2 = 0;
            for (int i = 0; i < num && i < count && i < 32; i++)
            {
                num2 = (bit[i + from] ? (num2 + (1 << i)) : num2);
            }

            return num2;
        }

        public static byte[] BitToByteArray(BitArray bitArray)
        {
            byte[] byteArray = new byte[(bitArray.Length - 1) / 8 + 1];
            bitArray.CopyTo(byteArray, 0);
            return byteArray;
        }


        public static BloomStats CalcOptimalMK(int expectedElements, double errorRate)
        {
            if (expectedElements < 1)
            {
                throw new ArgumentOutOfRangeException("expectedElements", expectedElements, "expectedElements must be > 0");
            }

            if (errorRate >= 1.0 || errorRate <= 0.0)
            {
                throw new ArgumentOutOfRangeException("errorRate", errorRate, $"errorRate must be between 0 and 1, exclusive. Was {errorRate}");
            }

            var capacity = BloomFilter.Filter.BestM(expectedElements, errorRate);
            var hashes = BloomFilter.Filter.BestK(expectedElements, capacity);


            return new BloomStats()
            {
                ExpectedElements = expectedElements,
                ErrorRate = errorRate,
                m = capacity,
                k = hashes
            };
        }

    }


    public class BloomStats
    {
        public int ExpectedElements { get; set; }
        public double ErrorRate { get; set; }
        public int m { get; set; }
        public int k { get; set; }


    }
}
