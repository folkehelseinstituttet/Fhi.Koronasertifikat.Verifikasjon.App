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
        public void Validate_WithMultiplePatients_IsInvalid()
        {
            wrapper.SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.Add(
                new SmartHealthCardPatient()
                );

            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Invalid, result);
        }

        [TestCase("2021-02-28")]
        [TestCase("1955-02")]
        [TestCase("1955")]
        public void Validate_WithValidBirthDate_IsValid(string date)
        {
            wrapper.SmartHealthCard.VerifiableCredential.CredentialSubject.Patients[0].DateOfBirth = date;

            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Valid, result);
        }

        [TestCase("2017-01-01T00:00:00.000Z")]
        [TestCase("2015-02-07T13:28:17-05:00")]
        [TestCase("date")]
        [TestCase("22")]
        public void Validate_WithInvalidBirthDate_IsInvalid(string date)
        {
            wrapper.SmartHealthCard.VerifiableCredential.CredentialSubject.Patients[0].DateOfBirth = date;

            TokenValidateResult result = wrapper.Validate();

            Assert.AreEqual(TokenValidateResult.Invalid, result);
        }
    }
}
