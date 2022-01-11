using System;
using System.Text.Json.Serialization;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class TestEntity
    {
        [JsonPropertyName("datetime")]
        [Newtonsoft.Json.JsonConverter(typeof(ISO8601DateTimeConverter))]
        public DateTime? DateTime { get; set; }
    }

    public class ISO8601DateTimeConverterTests
    {
        [TestCase("1951-02-01 00:00:00", "{\"datetime\":\"1951-02-01\"}")]
        [TestCase("1951-03-01 00:00:00", "{\"datetime\":\"1951-03\"}")]
        [TestCase("1951-01-01 00:00:00", "{\"datetime\":\"1951\"}")]
        [TestCase("2021-02-28 05:50:06", "{\"datetime\":\"2021-02-28 05:50:06\"}")]
        [TestCase("2015-02-07 19:28:17", "{\"datetime\":\"2015-02-07T13:28:17-05:00\"}")]
        [TestCase("2017-01-01 00:00:00", "{\"datetime\":\"2017-01-01T00:00:00.000Z\"}")]
        public void ISO8601DateTimeConverter_ShouldParseJsonToDateTime(DateTime expectedDate, string dateTime)
        {
            var result = JsonConvert.DeserializeObject<TestEntity>(dateTime);

            Assert.AreEqual(expectedDate, result.DateTime);
        }
    }
}
