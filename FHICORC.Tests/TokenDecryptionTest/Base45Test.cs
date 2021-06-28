using System;
using NUnit.Framework;
using FHICORC.Core.Services.Utils;

namespace FHICORC.Tests.TokenDecryptionTest
{
    public class Base45Test
    {
        [Theory]
        [TestCase("", "")]
        [TestCase("0", "31")]
        [TestCase("AB", "BB8")]
        [TestCase("Hello!!", "%69 VD92EX0")]
        [TestCase("base-45", "UJCLQE7W581")]
        public void Base45Encode(string input, string output)
        {
            var buffer = System.Text.Encoding.ASCII.GetBytes(input);
            var Base45Encoded = buffer.Base45Encode();
            Assert.AreEqual(output, Base45Encoded);
        }

        [Test]
        public void Boundary45AreEqual()
        {
            var buffer = new byte[] { 45 };
            var Base45Encoded = buffer.Base45Encode();
            Assert.AreEqual(2, Base45Encoded.Length);
            var decoded = Base45Encoded.Base45Decode();
            Assert.AreEqual(buffer, decoded);
        }

        [Theory]
        [TestCase(0, "")]
        [TestCase(1, "00")]
        [TestCase(10, "000000000000000")]
        public void Base45EncodeArrayOfZeros(int len, string output)
        {
            var Base45Encoded = new byte[len].Base45Encode();
            Assert.AreEqual(output, Base45Encoded);
        }

        [Theory]
        [TestCase(0, "")]
        [TestCase(1, "00")]
        [TestCase(10, "000000000000000")]
        public void DecodeArrayOfZeros(int len, string output)
        {
            var decoded = output.Base45Decode();
            Assert.AreEqual(new byte[len], decoded);
        }

        [Theory]
        [TestCase("", "")]
        [TestCase("0", "31")]
        [TestCase("AB", "BB8")]
        [TestCase("Hello!!", "%69 VD92EX0")]
        [TestCase("base-45", "UJCLQE7W581")]
        public void Base45Decode(string output, string input)
        {
            var decoded = input.Base45Decode();
            Assert.AreEqual(output, System.Text.Encoding.ASCII.GetString(decoded));
        }

        [Test]
        public void Base45EncodeNullGoBang()
        {
            Assert.Throws<ArgumentNullException>(() => Base45Extension.Base45Encode(null));
        }

        [Test]
        public void DecodeNullGoBang()
        {
            Assert.Throws<ArgumentNullException>(() => Base45Extension.Base45Decode(null));
        }

        [Theory]
        [TestCase("1")]
        [TestCase("1234")]
        [TestCase("1234567")]
        public void DecodeInvalidLengthGoBang(string input)
        {
            var ex = Assert.Throws<FormatException>(() => input.Base45Decode());
            Assert.AreEqual("Incorrect length.", ex.Message);
        }

        [Theory]
        [TestCase("^^^")]
        [TestCase("^^^^^^")]
        public void DecodeInvalidCharacterGoBang(string input)
        {
            var ex = Assert.Throws<FormatException>(() => input.Base45Decode());
            Assert.True(ex.Message.Contains("Invalid character at position"));
        }
    }
}