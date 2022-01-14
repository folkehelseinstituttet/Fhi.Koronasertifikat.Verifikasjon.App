using System;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Tests.Factories;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class CodeableConceptTests
    {
        [Test]
        public void Equals_WithEqualCoding_AreEqual()
        {
            CodeableConcept codeableConcept1 = SmartHealthCardFactory.CreateCodeableConcept();
            CodeableConcept codeableConcept2 = SmartHealthCardFactory.CreateCodeableConcept();

            Assert.AreEqual(codeableConcept1, codeableConcept2);
        }

        [Test]
        public void Equals_WithEqualCoding_DifferentOrder_AreEqual()
        {
            CodeableConcept codeableConcept1 = SmartHealthCardFactory.CreateCodeableConcept();
            CodeableConcept codeableConcept2 = SmartHealthCardFactory.CreateCodeableConcept();
            Array.Reverse(codeableConcept2.Coding);

            Assert.AreEqual(codeableConcept1, codeableConcept2);
        }

        [Test]
        public void Equals_WithDifferentCoding_AreNotEqual()
        {
            CodeableConcept codeableConcept1 = SmartHealthCardFactory.CreateCodeableConcept();
            CodeableConcept codeableConcept2 = SmartHealthCardFactory.CreateCodeableConcept();
            codeableConcept2.Coding[1].System = "another system";

            Assert.AreNotEqual(codeableConcept1, codeableConcept2);
        }
    }
}
