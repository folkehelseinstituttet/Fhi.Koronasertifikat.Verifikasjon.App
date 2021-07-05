using System;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.BusinessRules;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using FHICORC.Tests.TestMocks;
using NUnit.Framework;

namespace FHICORC.Tests.ServiceTests
{
    public class RuleSelectorServiceTest
    {
        private readonly IRuleSelectorService ruleSelectorService = new RuleSelectorService(
            DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator,
            new MockDateTimeService(),
            new MockBusinessRulesService());

        [Test]
        public void SelectInternational_WithVaccineReturnsOnlyVRAndGR()
        {
            var vaccinations = new Vaccination[1];
            vaccinations[0] = new Vaccination();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Vaccinations = vaccinations } } };
            var rules = ruleSelectorService.SelectRules(dcc, true);
            Assert.AreEqual(3, rules.Count);
        }

        [Test]
        public void SelectDomestic_WithVaccineReturnsOnlyVRAndGR()
        {
            var vaccinations = new Vaccination[1];
            vaccinations[0] = new Vaccination();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Vaccinations = vaccinations } } };
            var rules = ruleSelectorService.SelectRules(dcc, false);
            Assert.AreEqual(3, rules.Count);
        }

        [Test]
        public void SelectInternational_WithTestReturnsOnlyTRAndGR()
        {
            var testResults = new TestResult[1];
            testResults[0] = new TestResult();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Tests = testResults } } };
            var rules = ruleSelectorService.SelectRules(dcc, true);
            Assert.AreEqual(4, rules.Count);
        }

        [Test]
        public void SelectDomestic_WithTestReturnsOnlyTRAndGR()
        {
            var testResults = new TestResult[1];
            testResults[0] = new TestResult();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Tests = testResults } } };
            var rules = ruleSelectorService.SelectRules(dcc, false);
            Assert.AreEqual(2, rules.Count);
        }

        [Test]
        public void SelectInternational_WithRecoveryReturnsOnlyRRAndGR()
        {
            var recoveries = new Recovery[1];
            recoveries[0] = new Recovery();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Recovery = recoveries } } };
            var rules = ruleSelectorService.SelectRules(dcc, true);
            Assert.AreEqual(5, rules.Count);
        }

        [Test]
        public void SelectDomestic_WithRecoveryReturnsOnlyRRAndGR()
        {
            var recoveries = new Recovery[1];
            recoveries[0] = new Recovery();
            var dcc = new DCCPayload { DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { Recovery = recoveries } } };
            var rules = ruleSelectorService.SelectRules(dcc, false);
            Assert.AreEqual(3, rules.Count);
        }

        [Test]
        public void ApplyExternalData_AllFieldsSet()
        {
            var dcc = new DCCPayload { IssueAt = new DateTime(2021, 06, 10), ExpirationTime = new DateTime(2021, 06, 21), DCCPayloadData = new HCertModel { DCC = new DCCSchemaV1 { } } };
            ExternalData external = ruleSelectorService.ApplyExternalData(dcc, true);
            Assert.AreEqual("NO", external.CountryCode);
            Assert.AreEqual(dcc.ExpirationTime, external.Exp);
            Assert.AreEqual(dcc.IssueAt, external.Iat);
            Assert.NotNull(external.ValidationClock);
            Assert.NotNull(external.ValueSets);
        }
    }
}
