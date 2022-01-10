using System;
using System.Linq;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class CredentialSubjectTests
    {
        public CredentialSubjectTests()
        {
        }

        [TestCase("Anyperson, John B.", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"John\",\"B.\"]}],\"birthDate\":\"1951-02-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL1.2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"207\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000001\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL1.2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"207\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000007\"}}]}")]
        [TestCase("Anyperson, Jane C.", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"Jane\",\"C.\"]}],\"birthDate\":\"1961\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000002\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000008\"}}]}")]
        [TestCase("Anyperson, James T.", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"James\",\"T.\"]}],\"birthDate\":\"1951-02\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Observation\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"final\",\"code\":{\"coding\":[{\"system\":\"http://loinc.org\",\"code\":\"94558-4\"}]},\"subject\":{\"reference\":\"resource:0\"},\"effectiveDateTime\":\"2021-02-17\",\"valueCodeableConcept\":{\"coding\":[{\"system\":\"http://snomed.info/sct\",\"code\":\"260373001\"}]},\"performer\":[{\"display\":\"ABC General Hospital\"}]}}]}")]
        [TestCase("Anyperson, James T.", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"James\",\"T.\"]}],\"birthDate\":\"1951-03-31 09:50:00\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Observation\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"final\",\"code\":{\"coding\":[{\"system\":\"http://loinc.org\",\"code\":\"94558-4\"}]},\"subject\":{\"reference\":\"resource:0\"},\"effectiveDateTime\":\"2021-02-17\",\"valueCodeableConcept\":{\"coding\":[{\"system\":\"http://snomed.info/sct\",\"code\":\"260373001\"}]},\"performer\":[{\"display\":\"ABC General Hospital\"}]}}]}")]

        public void CredentialSubject_Patient_FullName_ShouldParseJson(string expected, string fhirBundleString)
        {
            string fhirVersion = "4.0.1";
            JObject fhirBundle = JObject.Parse(fhirBundleString);

            CredentialSubject credentialSubject = new CredentialSubject(fhirVersion, fhirBundle);
            Console.WriteLine(credentialSubject.Patients.First().DateOfBirth);

            Assert.AreEqual(expected, credentialSubject.Patients.First().PersonName.FullName);
        }

        [TestCase("completed", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"John\",\"B.\"]}],\"birthDate\":\"1951-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL1.2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"207\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000001\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL1.2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"207\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000007\"}}]}")]
        [TestCase("completed", "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"Jane\",\"C.\"]}],\"birthDate\":\"1961-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000002\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000008\"}}]}")]
        public void CredentialSubject_Immunization_Status_ShouldParseJson(string expected, string fhirBundleString)
        {
            string fhirVersion = null;
            JObject fhirBundle = JObject.Parse(fhirBundleString);

            CredentialSubject credentialSubject = new CredentialSubject(fhirVersion, fhirBundle);

            Assert.AreEqual(expected, credentialSubject.Immunizations.First().Status);
        }

        [TestCase("{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"Jane\",\"C.\"]}],\"birthDate\":\"1961-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000002\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000008\"}}]}")]
        [TestCase("{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"Jane\",\"C.\"]}],\"birthDate\":\"1961-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Specimen\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000002\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Specimen\",\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000008\"}}]}")]
        public void CredentialSubject_UnknownType_DoesNotThrowException(string fhirBundleString)
        {
            string fhirVersion = null;
            JObject fhirBundle = JObject.Parse(fhirBundleString);

            Assert.DoesNotThrow(() =>
            {
                CredentialSubject credentialSubject = new CredentialSubject(fhirVersion, fhirBundle);
            });
        }

        [TestCase(2,1,0, "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"Jane\",\"C.\"]}],\"birthDate\":\"1961-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Immunization\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-01\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000002\"}},{\"fullUrl\":\"resource:2\",\"resource\":{\"resourceType\":\"Immunization\",\"status\":\"completed\",\"vaccineCode\":{\"coding\":[{\"system\":\"http://hl7.org/fhir/sid/cvx\",\"code\":\"208\"}]},\"patient\":{\"reference\":\"resource:0\"},\"occurrenceDateTime\":\"2021-01-29\",\"performer\":[{\"actor\":{\"display\":\"ABC General Hospital\"}}],\"lotNumber\":\"0000008\"}}]}")]
        [TestCase(0,1,1, "{\"resourceType\":\"Bundle\",\"type\":\"collection\",\"entry\":[{\"fullUrl\":\"resource:0\",\"resource\":{\"resourceType\":\"Patient\",\"name\":[{\"family\":\"Anyperson\",\"given\":[\"James\",\"T.\"]}],\"birthDate\":\"1951-01-20\"}},{\"fullUrl\":\"resource:1\",\"resource\":{\"resourceType\":\"Observation\",\"meta\":{\"security\":[{\"system\":\"https://smarthealth.cards/ial\",\"code\":\"IAL2\"}]},\"status\":\"final\",\"code\":{\"coding\":[{\"system\":\"http://loinc.org\",\"code\":\"94558-4\"}]},\"subject\":{\"reference\":\"resource:0\"},\"effectiveDateTime\":\"2021-02-17\",\"valueCodeableConcept\":{\"coding\":[{\"system\":\"http://snomed.info/sct\",\"code\":\"260373001\"}]},\"performer\":[{\"display\":\"ABC General Hospital\"}]}}]}")]
        public void CredentialSubject_ShouldHaveCorrectResourceTypeCount(int expectedImmunizations, int expectedPatients, int expectedObservations, string fhirBundleString)
        {
            string fhirVersion = null;
            JObject fhirBundle = JObject.Parse(fhirBundleString);

            CredentialSubject credentialSubject = new CredentialSubject(fhirVersion, fhirBundle);
            Assert.AreEqual(expectedImmunizations, credentialSubject.Immunizations.Count);
            Assert.AreEqual(expectedPatients, credentialSubject.Patients.Count);
            Assert.AreEqual(expectedObservations, credentialSubject.Observations.Count);
        }
    }
}
