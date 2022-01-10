using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FHICORC.Core.Services.Model.Converter
{
    /*
     * Custom DateTime converter to support dates without days and months
     */

    public class ISO8601DateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Regex yearOnly = new Regex(@"\d{4}");
            Regex yearMonth = new Regex(@"\d{4}-\d{2}");

            if (yearMonth.IsMatch((string)reader.Value))
            {
                return DateTime.Parse(reader.Value.ToString() + "-01");
            }
            else if (yearOnly.IsMatch((string)reader.Value))
            {
                return DateTime.Parse(reader.Value.ToString() + "-01-01");
            }
            else
            {
                return DateTime.Parse(reader.Value.ToString());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd"));
        }
    }
}
