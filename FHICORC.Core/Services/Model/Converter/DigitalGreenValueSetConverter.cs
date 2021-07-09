using System;
using Newtonsoft.Json;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;

namespace FHICORC.Core.Services.Model.EuDCCModel.ValueSet
{
    public class DigitalGreenValueSetConverter : JsonConverter<string>
    {
        private readonly DGCValueSetEnum _key;
        private readonly IDgcValueSetTranslator _dgcValueSetTranslator;

        public DigitalGreenValueSetConverter(DGCValueSetEnum key, IDigitalGreenValueSetTranslatorFactory digitalGreenValueSetTranslatorFactory)
        {
            _dgcValueSetTranslator = digitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
            _key = key;
        }
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteRawValue(_dgcValueSetTranslator.GetDGCCode(_key, $"\"{value}\""));
        }

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return (string) _dgcValueSetTranslator.Translate(_key, reader.Value.ToString());
        }
    }
}