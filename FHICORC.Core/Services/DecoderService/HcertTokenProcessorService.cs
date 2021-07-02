using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.CoseModel;
using FHICORC.Core.Services.Model.EuDCCModel;
using FHICORC.Core.Services.Utils;
using FHICORC.Core.Services.Model.NO;
using System.Linq;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;
using FHICORC.Core.Services.Model.BusinessRules;
using System.Collections.Generic;
using FHICORC.Core.Data;

namespace FHICORC.Core.Services.DecoderServices
{
    public class HcertTokenProcessorService: ITokenProcessorService
    {
        private readonly ICertificationService _certificationService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRuleVerifierService _ruleVerifierService;
        private readonly IRuleSelectorService _ruleSelectorService;
        private readonly IPreferencesService _preferencesService;

        private IDgcValueSetTranslator _translator;

        public HcertTokenProcessorService(
            ICertificationService certificationService,
            IDateTimeService dateTimeService,
            IRuleSelectorService ruleSelectorService,
            IRuleVerifierService ruleVerifierService,
            IPreferencesService preferencesService)
        {
            _certificationService = certificationService;
            _dateTimeService = dateTimeService;
            _ruleSelectorService = ruleSelectorService;
            _ruleVerifierService = ruleVerifierService;
            _preferencesService = preferencesService;
            _translator = DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
            DigitalGreenValueSetTranslatorFactory.Init();
        }

        public void SetDgcValueSetTranslator(IDgcValueSetTranslator translator)
        {
            _translator = translator;
            DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator = translator;
            DigitalGreenValueSetTranslatorFactory.Init();
        }

        public async Task<TokenValidateResultModel> DecodePassportTokenToModel(string base45String)
        {
            TokenValidateResultModel resultModel = new TokenValidateResultModel();
            try
            {
                if (string.IsNullOrEmpty(base45String))
                {
                    return resultModel;
                }
                //Decode token to a cose sign 1 object
                CoseSign1Object coseSign1Object = DecodeToCOSEFlow(base45String);
#if !UNITTESTS
                await _certificationService.VerifyCoseSign1Object(coseSign1Object);
#endif
                string jsonStringFromBytes = coseSign1Object.GetJson();

                ITokenPayload decodedModel = MapToModelFromJson(jsonStringFromBytes, base45String.Substring(0, 3));

                DateTime? expiration = decodedModel.ExpiredDateTime();
                DateTime? issueAt = decodedModel.IssueDateTime();

                if (decodedModel is DCCPayload dccPayload)
                    resultModel.RulesFeedBacks = VerifyRules(dccPayload);

                if (expiration != null)
                {
                    if (_dateTimeService.Now.CompareTo(issueAt) <= 0)
                    {
                        resultModel.ValidationResult = TokenValidateResult.Invalid;
                    }
                    else if (_dateTimeService.Now.CompareTo(expiration) >= 0)
                    {
                        resultModel.ValidationResult = TokenValidateResult.Expired;
                    }
                    else
                    {
                        resultModel.ValidationResult = TokenValidateResult.Valid;
                    }
                }
                else
                {
                    //Assume the token are invalid if no expiration date found
                    resultModel.ValidationResult = TokenValidateResult.Invalid;
                }
                resultModel.DecodedModel = decodedModel;
                return resultModel;
            }
            catch (Exception)
            {
                // If any exceptions are throw, assume it invalid
                return resultModel;
            }
            
        }

        private List<RulesFeedbackData> VerifyRules(DCCPayload dccPayload)
        {
            bool international = _preferencesService.GetUserPreferenceAsBoolean("BORDER_CONTROL_ON");
            var external = _ruleSelectorService.ApplyExternalData(dccPayload);
            var applicableRules = _ruleSelectorService.SelectRules(dccPayload, international);
            var verifyRulesModel = new VerifyRulesModel { HCert = dccPayload.DCCPayloadData.DCC, External = external };
            
            return _ruleVerifierService.Verify(applicableRules, verifyRulesModel) ?? new List<RulesFeedbackData>();
        }

        private CoseSign1Object DecodeToCOSEFlow(string base45String)
        {
            // The app only expect passport with these prefix
            if (TokenTypeExtension.GetTokenType(base45String.Substring(0,3)) == TokenType.Unknown)
            {
                throw new InvalidDataException("The provided token is not a valid DK token or token based on hcert 1 specification");
            }
            //Remove the header
            base45String = base45String.Substring(3);
            if (base45String.StartsWith(":"))
            {
                base45String = base45String.Substring(1);
            }
            
            byte[] compressedBytesFromBase45Token = base45String.Base45Decode();
            
            byte[] decompressedSignedCOSEBytes = ZlibCompressionUtils.Decompress(compressedBytesFromBase45Token);
            
            
            CoseSign1Object cborMessageFromCOSE = CoseSign1Object.Decode(decompressedSignedCOSEBytes);
            return cborMessageFromCOSE;
        }

        private ITokenPayload MapToModelFromJson(string json, string tokenPrefix)
        {
            switch (TokenTypeExtension.GetTokenType(tokenPrefix))
            {
                case TokenType.NO1:
                    DefaultCWTPayload defaultPayload = JsonConvert.DeserializeObject<DefaultCWTPayload>(json);
                    return GetNODigitalGreenModelV1ByVersion(defaultPayload, json);
                case TokenType.HC1:
                    var deserialized = JsonConvert
                        .DeserializeObject<DCCPayload>(json);
                    ValidateCwt(deserialized);
                    return deserialized;
            }
            throw new InvalidDataException("The provided token is not a valid NO token or token based on hcert 1 specification");
        }

        private ITokenPayload GetNODigitalGreenModelV1ByVersion(DefaultCWTPayload defaultCwtPayload, string json)
        {
            //override the current translator in case multiple translator exist 
            DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator = _translator;
            switch (defaultCwtPayload.DGCPayloadData.euHcertV1Schema.Version)
            {
                case "1.0.0":
                    var deserialized = JsonConvert
                        .DeserializeObject<NO1Payload>(json);
                    return deserialized;
            }
            var exception = new InvalidDataException("the provided DGC token version is not implemented");

            throw exception;
        }

        private void ValidateCwt(DCCPayload cwt)
        {
            if (string.IsNullOrEmpty(cwt.DCCPayloadData.DCC.PersonName.StandardisedSurname))
            {
                throw new InvalidDataException("no transliterated surname is provided");
            }
            if (cwt.DCCPayloadData.DCC.Vaccinations?.Any() ?? false)
            {
                bool validDoseNumber = cwt.DCCPayloadData.DCC.Vaccinations.First().DoseNumber > 0;
                if (!validDoseNumber)
                    throw new InvalidDataException("invalid dose number");
            }
        }
    }
}