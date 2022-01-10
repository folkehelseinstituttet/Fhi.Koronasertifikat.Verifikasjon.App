using System;
using Newtonsoft.Json;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class VerifiableCredentialTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is VerifiableCredentialType VerifiableCredentialType)
            {
                writer.WriteValue(VerifiableCredentialType.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value is object)
            {
                string VerifiableCredentialTypeString = (string)reader.Value;
                if (VerifiableCredentialTypeSupport.VerifiableCredentialTypeDictionary.ContainsKey(VerifiableCredentialTypeString))
                {
                    return VerifiableCredentialTypeSupport.VerifiableCredentialTypeDictionary[VerifiableCredentialTypeString];
                }
                else
                {
                    throw new Exception($"One of the VerifiableCredentialTypes (vc.type) was not an allowed value, type found was: {VerifiableCredentialTypeString}. The supported types are: {GetSupportedVerifiableCredentialTypeUriStringList()}");
                }
            }
            else
            {
                throw new Exception($"Must provide at least one VerifiableCredentialTypes (vc.type). The supported types are: {GetSupportedVerifiableCredentialTypeUriStringList()}");
            }
        }

        private static string GetSupportedVerifiableCredentialTypeUriStringList()
        {
            string AllowedTypesList = string.Empty;
            foreach (var item in VerifiableCredentialTypeSupport.VerifiableCredentialTypeDictionary)
            {
                AllowedTypesList += $"{item.Key}, ";
            }
            AllowedTypesList = AllowedTypesList.Trim().TrimEnd(',');
            return AllowedTypesList;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
