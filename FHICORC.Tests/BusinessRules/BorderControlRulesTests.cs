using System;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services;
using FHICORC.Core.Services.BusinessRules;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using FHICORC.Tests.TestMocks;
using NUnit.Framework;

namespace FHICORC.Tests.BusinessRules
{
    public class BorderControlRulesTests
    {
        private IRuleSelectorService ruleSelectorService = new RuleSelectorService(
                DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator,
                new MockDateTimeService(),
                new MockBusinessRulesService()
            );

        private readonly IRuleVerifierService ruleVerifierService = new RuleVerifierService();

        private int ONE_OF_TWO_MIN_DAYS = -21;
        private int ONE_OF_TWO_MAX_DAYS = -99;

        private int TWO_OF_TWO_MIN_DAYS = -7;
        private int TWO_OF_TWO_MAX_DAYS = -9001; // No max?

        private int ONE_OF_ONE_MIN_DAYS = -21;
        private int ONE_OF_ONE_TWO_OF_TWO_TYPE_MIN_DAYS = -7;
        private int ONE_OF_ONE_MAX_DAYS = -9001; // No max?

        private int RECOVERY_MIN_DAYS = -10;
        private int RECOVERY_MAX_DAYS = -180; 

        private int TESTRESULT_MAX_HOURS = -24;

        private DCCPayload GetVaccinePayload(int daysUntilVaccinationDate)
        {
            return new DCCPayload
            {
                IssueAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddDays(14),
                DCCPayloadData = new HCertModel
                {
                    DCC = new DCCSchemaV1
                    {
                        DateOfBirth = "1990-01-01",
                        Vaccinations = new Vaccination[]
                        {
                            new Vaccination
                            {
                                DoseNumber = 2,
                                TotalSeriesOfDose = 2,
                                VaccineMedicinalProduct = "EU/1/20/1528",
                                DateOfVaccination = DateTime.UtcNow.AddDays(daysUntilVaccinationDate).ToString("O")
                            }
                        }
                    }
                }
            };
        }

        private DCCPayload GetRecoveryPayload(int daysUntilFirstPositive, int daysUntilValidFrom, int daysUntilValidTo)
        {
            return new DCCPayload
            {
                IssueAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddDays(14),
                DCCPayloadData = new HCertModel
                {
                    DCC = new DCCSchemaV1
                    {
                        DateOfBirth = "1990-01-01",
                        Recovery = new Recovery[]
                        {
                            new Recovery
                            {
                                DateOfFirstPositiveResult = DateTime.UtcNow.AddDays(daysUntilFirstPositive).ToString("O"),
                                Disease = "840539006",
                                ValidFrom = DateTime.UtcNow.AddDays(daysUntilValidFrom).ToString("d"),
                                ValidTo = DateTime.UtcNow.AddDays(daysUntilValidTo).ToString("d")
                            }
                        }
                    }
                }
            };
        }

        private DCCPayload GetTestResultPayload(int hoursUntilSampleCollectedTime)
        {
            return new DCCPayload
            {
                IssueAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddDays(14),
                DCCPayloadData = new HCertModel
                {
                    DCC = new DCCSchemaV1
                    {
                        DateOfBirth = "1990-01-01",
                        Tests = new TestResult[]
                        {
                            new TestResult
                            {
                                SampleCollectedTime = DateTime.UtcNow.AddHours(hoursUntilSampleCollectedTime),
                                ResultOfTest = "260415000",
                                Disease = "840539006"
                            }
                        }
                    }
                }
            };
        }

        private VerifyRulesModel GetVerifyRulesModel(DCCPayload dccPayload)
        {
            return new VerifyRulesModel
            {
                External = new ExternalData
                {
                    CountryCode = "NO",
                    Exp = dccPayload.ExpirationTime,
                    Iat = dccPayload.IssueAt,
                    ValidationClock = DateTime.UtcNow
                },
                HCert = dccPayload.DCCPayloadData.DCC
            };
        }

        [Test]
        public void Vaccine_OneOfTwoDoses_InValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_TWO_MIN_DAYS);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 2;
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfTwoDoses_BeforeValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_TWO_MIN_DAYS + 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 2;
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfTwoDoses_AfterValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_TWO_MAX_DAYS + 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 2;
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfTwoDoses_UnknownType_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_TWO_MIN_DAYS);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "UnknownType";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_TwoOfTwoDoses_InValidPeriod_Comirnaty_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MIN_DAYS - 1);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Vaccine_TwoOfTwoDoses_InValidPeriod_Moderna_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1507";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Vaccine_TwoOfTwoDoses_InValidPeriod_Vaxzevria_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/21/1529";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }


        [Test]
        public void Vaccine_TwoOfTwoDoses_BeforeValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MIN_DAYS + 1);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        [Ignore("No max period for vaccines yet")]
        public void Vaccine_TwoOfTwoDoses_AfterValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MAX_DAYS - 1);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_TwoOfTwoDoses_UnknownType_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(TWO_OF_TWO_MAX_DAYS + 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "UnknownType";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfOneDoses_InValidPeriod_Janssen_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1525";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Vaccine_OneOfOneDoses_BeforeValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_MIN_DAYS + 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1525";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfOneDoses_TwoOfTwoType_InValidPeriod_Comirnaty_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_TWO_OF_TWO_TYPE_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1528";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Vaccine_OneOfOneDoses_TwoOfTwoType_InValidPeriod_Moderna_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_TWO_OF_TWO_TYPE_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1507";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Vaccine_OneOfOneDoses_TwoOfTwoType_InValidPeriod_Vaxzevria_GreenResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_TWO_OF_TWO_TYPE_MIN_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/21/1529";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        [Ignore("No max period for vaccines yet")]
        public void Vaccine_OneOfOneDoses_AfterValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_MAX_DAYS - 1);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "EU/1/20/1525";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Vaccine_OneOfOneDoses_UnknownType_RedResult()
        {
            DCCPayload dccPayload = GetVaccinePayload(ONE_OF_ONE_MIN_DAYS);
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].DoseNumber = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].TotalSeriesOfDose = 1;
            dccPayload.DCCPayloadData.DCC.Vaccinations[0].VaccineMedicinalProduct = "UNKNOWN_TYPE";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Recovery_InValidPeriod_GreenResult()
        {
            DCCPayload dccPayload = GetRecoveryPayload(RECOVERY_MIN_DAYS - 1, 0, RECOVERY_MAX_DAYS);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Recovery_BeforeValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetRecoveryPayload(RECOVERY_MIN_DAYS + 1, 0, RECOVERY_MAX_DAYS);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            bool atLeastOneFalse = false;
            foreach (var result in results)
            {
                if (result.Result == RulesFeedbackResult.FALSE)
                {
                    atLeastOneFalse = true;
                    break;
                }
            }
            Assert.AreEqual(true, atLeastOneFalse);
        }

        [Test]
        public void Recovery_AfterValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetRecoveryPayload(RECOVERY_MIN_DAYS + 1, RECOVERY_MAX_DAYS, RECOVERY_MAX_DAYS);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            rulesModel.External.ValidationClock = DateTime.UtcNow.AddDays(RECOVERY_MAX_DAYS);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.FALSE, result.Result);
            }
        }

        [Test]
        public void TestResult_InValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetTestResultPayload(TESTRESULT_MAX_HOURS);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.FALSE, result.Result);
            }
        }

        [Test]
        public void TestResult_AfterValidPeriod_RedResult()
        {
            DCCPayload dccPayload = GetTestResultPayload(TESTRESULT_MAX_HOURS - 1);
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.FALSE, result.Result);
            }
        }

        [Test]
        public void TestResult_InValidPeriod_UnknownType_RedResult()
        {
            DCCPayload dccPayload = GetTestResultPayload(TESTRESULT_MAX_HOURS);
            dccPayload.DCCPayloadData.DCC.Tests[0].Disease = "UNKNOWN";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.FALSE, result.Result);
            }
        }

        [Test]
        public void TestResult_AfterValidPeriod_UnknownType_RedResult()
        {
            DCCPayload dccPayload = GetTestResultPayload(TESTRESULT_MAX_HOURS - 1);
            dccPayload.DCCPayloadData.DCC.Tests[0].Disease = "UNKNOWN";
            VerifyRulesModel rulesModel = GetVerifyRulesModel(dccPayload);
            var rules = ruleSelectorService.SelectRules(dccPayload, true);
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.FALSE, result.Result);
            }
        }
    }
}
