using System;
using System.Collections.Generic;

namespace FHICORC.Core.Services.Utils
{
    public static class Base45Extension
    {
        private const int BaseSize = 45;
        private const int BaseSizeSquared = 2025;
        private const int ChunkSize = 2;
        private const int EncodedChunkSize = 3;
        private const int SmallEncodedChunkSize = 2;
        private const int ByteSize = 256;

        private static readonly char[] _Encoding = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                                    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                                                    'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*',
                                                    '+', '-', '.', '/', ':' };

        private readonly static Dictionary<char, byte> _Decoding = new Dictionary<char, byte>(BaseSize);

        static Base45Extension()
        {
            for (byte i = 0; i < _Encoding.Length; ++i)
                _Decoding.Add(_Encoding[i], i);
        }

        public static string Base45Encode(this byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            var wholeChunkCount = buffer.Length / ChunkSize;
            var result = new char[wholeChunkCount * EncodedChunkSize + (buffer.Length % ChunkSize == 1 ? SmallEncodedChunkSize : 0)];

            if (result.Length == 0)
                return string.Empty;

            var resultIndex = 0;
            var wholeChunkLength = wholeChunkCount * ChunkSize;
            for (var i = 0; i < wholeChunkLength;)
            {
                var value = buffer[i++] * ByteSize + buffer[i++];
                result[resultIndex++] = _Encoding[value % BaseSize];
                result[resultIndex++] = _Encoding[value / BaseSize % BaseSize];
                result[resultIndex++] = _Encoding[value / BaseSizeSquared % BaseSize];
            }

            if (buffer.Length % ChunkSize == 0)
                return new string(result);

            result[result.Length - 2] = _Encoding[buffer[buffer.Length - 1] % BaseSize];
            result[result.Length - 1] = buffer[buffer.Length - 1] < BaseSize ? _Encoding[0] : _Encoding[buffer[buffer.Length - 1] / BaseSize % BaseSize];

            return new string(result);
        }

        public static byte[] Base45Decode(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                return Array.Empty<byte>();

            var remainderSize = value.Length % EncodedChunkSize;
            if (remainderSize == 1)
                throw new FormatException("Incorrect length.");

            var buffer = new byte[value.Length];
            for (var i = 0; i < value.Length; ++i)
            {
                if (_Decoding.TryGetValue(value[i], out var decoded))
                {
                    buffer[i] = decoded;
                    continue; //Earliest return on expected path.
                }

                throw new FormatException($"Invalid character at position {i}.");
            }

            var wholeChunkCount = buffer.Length / EncodedChunkSize;
            var result = new byte[wholeChunkCount * ChunkSize + (remainderSize == ChunkSize ? 1 : 0)];
            var resultIndex = 0;
            var wholeChunkLength = wholeChunkCount * EncodedChunkSize;
            for (var i = 0; i < wholeChunkLength;)
            {
                var val = buffer[i++] + BaseSize * buffer[i++] + BaseSizeSquared * buffer[i++];
                result[resultIndex++] = (byte)(val / ByteSize); //result is always in the range 0-255 - % ByteSize omitted.
                result[resultIndex++] = (byte)(val % ByteSize);
            }

            if (remainderSize == 0)
                return result;

            result[result.Length - 1] = (byte)(buffer[buffer.Length - 2] + BaseSize * buffer[buffer.Length - 1]); //result is always in the range 0-255 - % ByteSize omitted.
            return result;
        }
    }
}
