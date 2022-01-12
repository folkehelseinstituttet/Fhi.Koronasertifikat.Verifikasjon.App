using System;
using System.Collections.Generic;
using FHICORC.Core.Services.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FHICORC.Core.Services.Model.SmartHealthCardModel.Shc
{
    public class CredentialSubject
    {
        [JsonConstructor]
        public CredentialSubject(string FhirVersion, JObject FhirBundle)
        {
            Immunizations = new List<SmartHealthCardImmunization>();
            Observations = new List<SmartHealthCardObservation>();
            Patients = new List<SmartHealthCardPatient>();

            this.FhirVersion = FhirVersion;
            JArray entries = (JArray)FhirBundle["entry"];

            foreach (JObject entry in entries)
            {
                string resourceType = (string)entry["resource"]["resourceType"];
                if (System.Enum.TryParse(resourceType, out ResourceType type))
                {
                    switch (type)
                    {
                        case ResourceType.Patient:
                            var patient = JsonConvert.DeserializeObject<SmartHealthCardPatient>(entry["resource"].ToString());
                            Patients.Add(patient);
                            break;
                        case ResourceType.Immunization:
                            var immunization = JsonConvert.DeserializeObject<SmartHealthCardImmunization>(entry["resource"].ToString());
                            Immunizations.Add(immunization);
                            break;
                        case ResourceType.Observation:
                            var observation = JsonConvert.DeserializeObject<SmartHealthCardObservation>(entry["resource"].ToString());
                            Observations.Add(observation);
                            break;
                    }
                }
            }
        }

        [JsonProperty("fhirVersion", Required = Required.Always)]
        public string FhirVersion { get; set; }

        [JsonIgnore]
        public List<SmartHealthCardPatient> Patients;

        [JsonIgnore]
        public List<SmartHealthCardImmunization> Immunizations;

        [JsonIgnore]
        public List<SmartHealthCardObservation> Observations;
    }
}
