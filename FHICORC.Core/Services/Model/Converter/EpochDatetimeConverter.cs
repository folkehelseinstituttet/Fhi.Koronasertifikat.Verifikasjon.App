using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FHICORC.Core.Services.Model.Converter
{
    static class EpochDatetimeConverterExtension
    {
        public static DateTimeOffset ToDateTimeOffset(this JsonReader reader)
        {
            string date = reader.Value?.ToString();

            if (date?.Contains(".") == true)
            {
                date = date.Substring(0, date.IndexOf("."));
            }
            else if (date?.Contains(",") == true)
            {
                date = date.Substring(0, date.IndexOf(","));
            }

            return date.Length > 10 ?
                DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(date!)) :
                DateTimeOffset.FromUnixTimeSeconds(long.Parse(date!));
        }
    }
    public class EpochDatetimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }

            string date = reader.Value.ToString();

            DateTimeOffset fromUnixTimeMilliseconds;
            if (date.Contains(".") || date.Contains(","))
            {
                fromUnixTimeMilliseconds = reader.ToDateTimeOffset();
            }
            else if (!DateTimeOffset.TryParse(date, out fromUnixTimeMilliseconds))
            {
                fromUnixTimeMilliseconds = reader.ToDateTimeOffset();
            }

            return fromUnixTimeMilliseconds.DateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }
    }
}