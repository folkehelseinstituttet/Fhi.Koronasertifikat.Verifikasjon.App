using System;
using System.Collections.Generic;
using System.Linq;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;

namespace FHICORC.Core.Services.BusinessRules
{
    public class RuleSelectorService : IRuleSelectorService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IBusinessRulesService _businessRulesService;
        private readonly IDigitalGreenValueSetTranslatorFactory _digitalGreenValueSetTranslatorFactory;

        public RuleSelectorService(
            IDateTimeService dateTimeService,
            IBusinessRulesService businessRulesService,
            IDigitalGreenValueSetTranslatorFactory digitalGreenValueSetTranslatorFactory)
        {
            _dateTimeService = dateTimeService;
            _businessRulesService = businessRulesService;
            _digitalGreenValueSetTranslatorFactory = digitalGreenValueSetTranslatorFactory;
        }

        public ExternalData ApplyExternalData(DCCPayload dccPayload, bool international)
        {
            ExternalData external = new ExternalData();
            ValueSets valueSets = new ValueSets();

            var _translator = _digitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;

            foreach (var valueSetModel in _translator.ValueSetModels)
            {
                switch (valueSetModel.ValueSetId)
                {
                    case "disease-agent-targeted":
                        valueSets.DiseaseAgentTargeted = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "covid-19-lab-result":
                        valueSets.Covid19LabResult = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "vaccines-covid-19-auth-holders":
                        valueSets.VaccinesCovid19AuthHolders = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "vaccines-covid-19-names":
                        valueSets.VaccinesCovid19Names = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "sct-vaccines-covid-19":
                        valueSets.SctVaccinesCovid19 = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "covid-19-lab-test-manufacturer-and-name":
                        valueSets.TestManufacturers = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                    case "covid-19-lab-test-type":
                        valueSets.TestTypes = valueSetModel.ValueSetValues.Keys.ToList();
                        break;
                }
            }

            external.ValidationClock = _dateTimeService.Now;
            external.CountryCode = international ? "NO" : "NX";
            external.Exp = dccPayload.ExpiredDateTime();
            external.Iat = dccPayload.IssueDateTime();
            external.ValueSets = valueSets;

            return external;
        }

        public ICollection<BusinessRule> SelectRules(DCCPayload dccPayload, bool international)
        {
            bool vaccinations = false;
            bool tests = false;
            bool recovery = false;

            vaccinations = dccPayload.DCCPayloadData.DCC.Vaccinations?.Any() ?? false;
            tests = dccPayload.DCCPayloadData.DCC.Tests?.Any() ?? false;
            recovery = dccPayload.DCCPayloadData.DCC.Recovery?.Any() ?? false;


            var businessRules = _businessRulesService.GetBusinessRules();
            List<BusinessRule> resultList = new List<BusinessRule>();
            string countryCode = international ? "NO" : "NX";
            string generalString = "GR-" + countryCode;
            if (vaccinations)
            {
                string vaccineStr = "VR-" + countryCode;
                resultList = businessRules.Where(b => b.Identifier.Contains(vaccineStr)).ToList();
                resultList.AddRange(businessRules.Where(b => b.Identifier.Contains(generalString)).ToList());
            }
            else if (tests)
            {
                string testStr = "TR-" + countryCode;
                resultList = businessRules.Where(b => b.Identifier.Contains(testStr)).ToList();
                resultList.AddRange(businessRules.Where(b => b.Identifier.Contains(generalString)).ToList());
            }
            else if (recovery)
            {
                string recoveryStr = "RR-" + countryCode;
                resultList = businessRules.Where(b => b.Identifier.Contains(recoveryStr)).ToList();
                resultList.AddRange(businessRules.Where(b => b.Identifier.Contains(generalString)).ToList());
            }

            return resultList;
        }
    }
}
