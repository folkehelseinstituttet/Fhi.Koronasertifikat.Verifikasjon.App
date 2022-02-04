using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FHICORC.Core.Services.Model.Converter
{
    /*
     * Strict DateTime converter
     * - To not allow missing month or day.
     * - To require timezone info, if time is provided.
     * 
     * Should be used together with json serializer setting DateParseHandling.None.
     * So input is string and not date.
     */

    public class StrictISO8601DateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            string date = reader.Value.ToString();
            Regex dateTimeRegex = new Regex(@"^([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\.[0-9]+)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)))?))$");
            if (!dateTimeRegex.IsMatch(date))
            {
                throw new FormatException("Date did not match regex");
            }
            return DateTime.Parse(date);
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd"));
        }
    }
}
