using System;
using System.Collections.Generic;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using NUnit.Framework;

namespace FHICORC.Tests.ServiceTests
{
    public class RuleVerifierServiceTests
    {
        private readonly IRuleVerifierService ruleVerifierService = new RuleVerifierService();

        [Test]
        public void Verify_VaccineRulePositive()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                DoseNumber = 1,
                                TotalSeriesOfDose = 1,
                                DateOfVaccination = DateTime.UtcNow.AddDays(-22).ToString("O")
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{ ""if"": [{ ""and"": [{ ""==="": [{ ""var"": ""payload.v.0.dn"" }, 1] }, { ""==="": [{ ""var"": ""payload.v.0.sd"" }, 1] } ] }, { ""after"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.v.0.dt"" }, 21, ""day""] } ] }, true ] }"
                }
            };
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Verify_TestRulePositive()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                SampleCollectedTime = DateTime.UtcNow.AddHours(-20)
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{ ""before"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.t.0.sc"" }, 24, ""hour""] } ] }"
                }
            };
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Verify_RecoveryRulePositive()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                DateOfFirstPositiveResult = DateTime.UtcNow.AddDays(-12).ToString("O")
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{""after"": [{ ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 183, ""day""] }, { ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 10, ""day""] } ]}"
                }
            };
            var results = ruleVerifierService.Verify(rules, rulesModel);
            foreach (var result in results)
            {
                Assert.AreEqual(RulesFeedbackResult.TRUE, result.Result);
            }
        }

        [Test]
        public void Verify_VaccineRuleNegative()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                DoseNumber = 1,
                                TotalSeriesOfDose = 1,
                                DateOfVaccination = DateTime.UtcNow.AddDays(-20).ToString("O")
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{ ""if"": [{ ""and"": [{ ""==="": [{ ""var"": ""payload.v.0.dn"" }, 1] }, { ""==="": [{ ""var"": ""payload.v.0.sd"" }, 1] } ] }, { ""after"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.v.0.dt"" }, 21, ""day""] } ] }, true ] }"
                }
            };
            var result = ruleVerifierService.Verify(rules, rulesModel);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(RulesFeedbackResult.FALSE, result[0].Result);
        }

        [Test]
        public void Verify_TestRuleNegative()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                SampleCollectedTime = DateTime.UtcNow.AddHours(-25)
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{ ""before"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.t.0.sc"" }, 24, ""hour""] } ] }"
                }
            };
            var result = ruleVerifierService.Verify(rules, rulesModel);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(RulesFeedbackResult.FALSE, result[0].Result);
        }

        [Test]
        public void Verify_RecoveryRuleNegative()
        {
            DCCPayload dccPayload = new DCCPayload
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
                                DateOfFirstPositiveResult = DateTime.UtcNow.AddDays(-9).ToString("O")
                            }
                        }
                    }
                }
            };
            VerifyRulesModel rulesModel = new VerifyRulesModel
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
            List<BusinessRule> rules = new List<BusinessRule>
            {
                new BusinessRule
                {
                    Logic = @"{""after"": [{ ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 183, ""day""] }, { ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 10, ""day""] } ]}"
                }
            };
            var result = ruleVerifierService.Verify(rules, rulesModel);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(RulesFeedbackResult.FALSE, result[0].Result);
        }
    }
}
