using NUnit.Framework;
using FHICORC.Core.Services.Interface;
using FHICORC.Tests.TestMocks;
using FHICORC.Tests.Factories;
using System.Collections.Generic;
using FHICORC.Core.Services.DecoderService;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using System.Threading.Tasks;
using System;

namespace FHICORC.Tests.ServiceTests
{
    public class CodingServiceTests
    {
        private readonly ICodingService codingService;

        public CodingServiceTests()
        {
            codingService = new CodingService(
                new MockSmartHealthCardRepository()
            );
        }

        [Test]
        public async Task GetShcVaccineInfo_OneImmunization_ReturnsSingleInfo()
        {
            SmartHealthCardImmunization immunization = SmartHealthCardFactory.CreateImmunization();

            List<SmartHealthCardVaccineInfo> vaccineInfo = await codingService.GetShcVaccineInfo(
                new List<SmartHealthCardImmunization> { immunization });

            Assert.AreEqual(1, vaccineInfo.Count);
            Assert.AreEqual(immunization.VaccineCode.Id, vaccineInfo[0].Id);
        }

        [Test]
        public async Task GetShcVaccineInfo_TwoEqualImmunizations_ReturnsSingleInfo()
        {
            SmartHealthCardImmunization immunization1 = SmartHealthCardFactory.CreateImmunization();
            SmartHealthCardImmunization immunization2 = SmartHealthCardFactory.CreateImmunization();

            List<SmartHealthCardVaccineInfo> vaccineInfo = await codingService.GetShcVaccineInfo(
                new List<SmartHealthCardImmunization> { immunization1, immunization2 });

            Assert.AreEqual(1, vaccineInfo.Count);
            Assert.AreEqual(immunization1.VaccineCode.Id, vaccineInfo[0].Id);
            Assert.AreEqual(immunization2.VaccineCode.Id, vaccineInfo[0].Id);
        }

        [Test]
        public async Task GetShcVaccineInfo_TwoEqualDifferentOrderImmunizations_ReturnsSingleInfoAsync()
        {
            SmartHealthCardImmunization immunization1 = SmartHealthCardFactory.CreateImmunization();
            SmartHealthCardImmunization immunization2 = SmartHealthCardFactory.CreateImmunization();
            Array.Reverse(immunization2.VaccineCode.Coding);

            List<SmartHealthCardVaccineInfo> vaccineInfo = await codingService.GetShcVaccineInfo(
                new List<SmartHealthCardImmunization> { immunization1, immunization2 });

            Assert.AreEqual(1, vaccineInfo.Count);
            Assert.AreEqual(immunization1.VaccineCode.Id, vaccineInfo[0].Id);
            Assert.AreEqual(immunization2.VaccineCode.Id, vaccineInfo[0].Id);
        }

        [Test]
        public async Task GetShcVaccineInfo_TwoDifferentImmunizations_ReturnsTwoInfo()
        {
            SmartHealthCardImmunization immunization1 = SmartHealthCardFactory.CreateImmunization();
            SmartHealthCardImmunization immunization2 = SmartHealthCardFactory.CreateImmunization();
            immunization2.VaccineCode.Coding[0].Code = "anotherCode";

            List<SmartHealthCardVaccineInfo> vaccineInfo = await codingService.GetShcVaccineInfo(
                new List<SmartHealthCardImmunization> { immunization1, immunization2 });

            Assert.AreEqual(2, vaccineInfo.Count);
            Assert.AreEqual(immunization1.VaccineCode.Id, vaccineInfo[0].Id);
            Assert.AreEqual(immunization2.VaccineCode.Id, vaccineInfo[1].Id);
        }
    }
}
