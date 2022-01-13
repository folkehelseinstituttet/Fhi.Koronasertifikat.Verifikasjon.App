using System.IO;
using System;
using NUnit.Framework;
using FHICORC.Core.Services.Model;

namespace FHICORC.Tests.ModelTests
{
    public class VerifiableCredentialTypeTests
    {
        public VerifiableCredentialTypeTests()
        {
        }

        [Test]
        public void VerifyType_MissingJsonKey_ThrowsException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{}}";

            Assert.Throws<NullReferenceException>(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }

        [Test]
        public void VerifyType_NoTypes_ThrowsException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{\"type\":[]}}";

            Assert.Throws<InvalidDataException>(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }

        [Test]
        public void VerifyType_OnlyCovid19Type_ThrowsException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{\"type\":[" +
                "\"https://smarthealth.cards#covid19\"]}}";

            Assert.Throws<InvalidDataException>(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }

        [Test]
        public void VerifyType_OnlyHealthCardType_ThrowsException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{\"type\":[" +
                "\"https://smarthealth.cards#health-card\"]}}";

            Assert.Throws<InvalidDataException>(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }

        [Test]
        public void VerifyType_Covid19AndHealthCardType_DoesNotThrowException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{\"type\":[" +
                "\"https://smarthealth.cards#health-card\"," +
                "\"https://smarthealth.cards#covid19\"]}}";

            Assert.DoesNotThrow(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }


        [Test]
        public void VerifyType_Covid19AndHealthCardType_AndUnkownType_DoesNotThrowException()
        {
            string json = "{\"iss\":\"iss\",\"nbf\":110,\"vc\":{\"type\":[" +
                "\"https://smarthealth.cards#health-card\"," +
                   "\"someUnknownType\"," +
                "\"https://smarthealth.cards#covid19\"]}}";

            Assert.DoesNotThrow(() =>
            {
                VerifiableCredentialTypeSupport.VerifyType(json);
            });
        }
    }
}
