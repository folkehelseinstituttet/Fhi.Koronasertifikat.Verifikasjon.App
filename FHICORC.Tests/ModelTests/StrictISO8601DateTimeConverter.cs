using System;
using FHICORC.Core.Services.Model.Converter;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class TestEntity
    {
        [JsonProperty("datetime")]
        [JsonConverter(typeof(StrictISO8601DateTimeConverter))]
        public DateTime DateTime { get; set; }
    }

    public class StrictISO8601DateTimeConverterTests
    {
        [TestCase("1951-02-01", "{\"datetime\":\"1951-02-01\"}")]
        [TestCase("2015-02-07 18:28:17-00:00", "{\"datetime\":\"2015-02-07T13:28:17-05:00\"}")]
        [TestCase("2015-02-07 08:28:17-00:00", "{\"datetime\":\"2015-02-07T13:28:17+05:00\"}")]
        [TestCase("2017-01-01 00:00:00-00:00", "{\"datetime\":\"2017-01-01T00:00:00.000Z\"}")]
        public void Convert_ShouldParseJsonToDateTime(string expectedDate, string dateTime)
        {
            DateTime expected = DateTime.Parse(expectedDate);

            TestEntity result = JsonConvert.DeserializeObject<TestEntity>(
                dateTime,
                new JsonSerializerSettings()
                {
                    DateParseHandling = DateParseHandling.None
                });

            Assert.AreEqual(expected, result.DateTime);
        }

        [TestCase("{\"datetime\":\"2021-02-28T05:50:06\"}")]
        [TestCase("{\"datetime\":\"2021-02-28 05:50:06\"}")]
        [TestCase("{\"datetime\":\"1951-03\"}")]
        [TestCase("{\"datetime\":\"1951\"}")]
        public void Convert_ShouldThrowException(string dateTime)
        {
            Assert.Throws<FormatException>(() =>
            {
                JsonConvert.DeserializeObject<TestEntity>(
                    dateTime,
                    new JsonSerializerSettings()
                    {
                        DateParseHandling = DateParseHandling.None
                    });
            });
        }
    }
}
