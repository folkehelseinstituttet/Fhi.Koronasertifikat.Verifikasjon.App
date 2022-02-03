using System;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Tests.Factories;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class SmartHealthCardWrapperTests
    {
        private SmartHealthCardWrapper wrapper;

        [SetUp]
        public void Init()
        {
            wrapper = SmartHealthCardFactory.CreateWrapper();
        }

        [Test]
        public void Validate_Valid()
        {
            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Valid, result);
        }

        [Test]
        public void Validate_WithExpiredDate_IsExpired()
        {
            wrapper.SmartHealthCard.ExpirationDate = new DateTime(2019, 1, 30, 0, 0, 0, DateTimeKind.Utc);

            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Expired, result);
        }

        [Test]
        public void Validate_WithMultiplePatiants_IsInvalid()
        {
            wrapper.SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.Add(
                new SmartHealthCardPatient()
                );

            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Invalid, result);
        }
    }
}
