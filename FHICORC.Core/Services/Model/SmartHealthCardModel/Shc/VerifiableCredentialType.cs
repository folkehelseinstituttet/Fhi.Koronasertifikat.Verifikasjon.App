using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FHICORC.Core.Services.Model
{
    public enum VerifiableCredentialType
    {
        HealthCard,
        Covid19,
        Immunization,
        Laboratory,
        Unknown
    }

    public static class VerifiableCredentialTypeSupport
    {
        public static readonly Dictionary<string, VerifiableCredentialType> VerifiableCredentialTypeDictionary = new Dictionary<string, VerifiableCredentialType>()
        {
          { "https://smarthealth.cards#covid19", VerifiableCredentialType.Covid19 },
          { "https://smarthealth.cards#health-card", VerifiableCredentialType.HealthCard },
          { "https://smarthealth.cards#immunization", VerifiableCredentialType.Immunization },
          { "https://smarthealth.cards#laboratory", VerifiableCredentialType.Laboratory },
          { "Unknown", VerifiableCredentialType.Unknown }
        };

        public static void VerifyType(string SmartHealthCard)
        {
            JObject payloadData = JObject.Parse(SmartHealthCard);
            JArray types = (JArray)payloadData["vc"]["type"];
            bool isCovid = false;
            bool isHealthCard = false;
            foreach (string item in types)
            {
                if (VerifiableCredentialTypeDictionary.TryGetValue(item, out var type))
                {
                    if (type == VerifiableCredentialType.Covid19)
                    {
                        isCovid = true;
                    }
                    if (type == VerifiableCredentialType.HealthCard)
                    {
                        isHealthCard = true;
                    }
                }
            }

            if (!isCovid)
            {
                throw new InvalidDataException("Smart health card types must contain covid19");
            }
            if (!isHealthCard)
            {
                throw new InvalidDataException("Smart health card types must contain health card");
            }
        }
    }
}