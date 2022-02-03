﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
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
using System.Text;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;
using System.Diagnostics;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;

namespace FHICORC.Core.Services.DecoderServices
{
    public class HcertTokenProcessorService: ITokenProcessorService
    {
        private readonly ICertificationService _certificationService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRuleVerifierService _ruleVerifierService;
        private readonly IRuleSelectorService _ruleSelectorService;
        private readonly IPreferencesService _preferencesService;
        private readonly IDigitalGreenValueSetTranslatorFactory _digitalGreenValueSetTranslatorFactory;
        private readonly ICodingService _codingService;
        private readonly IConnectivityService _connectivityService;

        private IDgcValueSetTranslator _translator;

        public HcertTokenProcessorService(
            ICertificationService certificationService,
            IDateTimeService dateTimeService,
            IRuleSelectorService ruleSelectorService,
            IRuleVerifierService ruleVerifierService,
            IPreferencesService preferencesService,
            IDigitalGreenValueSetTranslatorFactory digitalGreenValueSetTranslatorFactory,
            ICodingService codingService,
            IConnectivityService connectivityService)
        {
            _certificationService = certificationService;
            _dateTimeService = dateTimeService;
            _ruleSelectorService = ruleSelectorService;
            _ruleVerifierService = ruleVerifierService;
            _preferencesService = preferencesService;
            _digitalGreenValueSetTranslatorFactory = digitalGreenValueSetTranslatorFactory;
            _translator = _digitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
            _digitalGreenValueSetTranslatorFactory.Init();
            _codingService = codingService;
            _connectivityService = connectivityService;
        }

        public void SetDgcValueSetTranslator(IDgcValueSetTranslator translator)
        {
            _translator = translator;
            _digitalGreenValueSetTranslatorFactory.DgcValueSetTranslator = translator;
            _digitalGreenValueSetTranslatorFactory.Init();
        }

        public async Task<TokenValidateResultModel> DecodeDCCPassportTokenToModel(string base45String)
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
                await _certificationService.VerifyCoseSign1Object(coseSign1Object);

                string jsonStringFromBytes = coseSign1Object.GetJson();
                bool international = _preferencesService.GetUserPreferenceAsBoolean("BORDER_CONTROL_ON");

                ITokenPayload decodedModel;
                if (international)
                    decodedModel = InternationalMapToModelFromJson(jsonStringFromBytes, base45String.Substring(0, 3));
                else
                    decodedModel = DomesticMapToModelFromJson(jsonStringFromBytes, base45String.Substring(0, 3));

                if (decodedModel == null)
                {
                    resultModel.ValidationResult = TokenValidateResult.UnsupportedType;
                    return resultModel;
                }

                DateTime? expiration = decodedModel.ExpiredDateTime();
                DateTime? issueAt = decodedModel.IssueDateTime();

                if (decodedModel is DCCPayload dccPayload)
                    resultModel.RulesFeedBacks = VerifyRules(dccPayload, international);

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

        private List<RulesFeedbackData> VerifyRules(DCCPayload dccPayload, bool international)
        {
            var external = _ruleSelectorService.ApplyExternalData(dccPayload, international);
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

        private ITokenPayload DomesticMapToModelFromJson(string json, string tokenPrefix)
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
            return null;
        }

        private ITokenPayload InternationalMapToModelFromJson(string json, string tokenPrefix)
        {
            switch (TokenTypeExtension.GetTokenType(tokenPrefix))
            {
                case TokenType.HC1:
                    var deserialized = JsonConvert
                        .DeserializeObject<DCCPayload>(json);
                    ValidateCwt(deserialized);
                    return deserialized;
            }
            return null;
        }

        private ITokenPayload GetNODigitalGreenModelV1ByVersion(DefaultCWTPayload defaultCwtPayload, string json)
        {
            //override the current translator in case multiple translator exist 
            _digitalGreenValueSetTranslatorFactory.DgcValueSetTranslator = _translator;
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

        public async Task<TokenValidateResultModel> DecodeSHCPassportTokenToModel(string qrCodeToken)
        {
            TokenValidateResultModel resultModel = new TokenValidateResultModel();

            try
            {
                // Step 1. Decode JWS token
                string jwsToken = DecodeSHCJwsToken(qrCodeToken);

                // Step 2. Split JWS Token into parts
                JwsParts jwsParts = JwsParts.ParseToken(jwsToken);

                // Step 3. Verify SHC COVID-19 type
                string SmartHealthCard = await jwsParts.DecodedPayload();
                VerifiableCredentialTypeSupport.VerifyType(SmartHealthCard);

                // Step 4. Verify signature
                await _certificationService.VerifySHCSignature(jwsParts);

                // Step 5. Verify that the issuer is trusted
                SmartHealthCardIssuer issuer = await _certificationService.VerifySHCIssuer(jwsParts);

                // Step 6. (Optional, not in scope) Revocation

                // Step 7. Create Models
                SmartHealthCardModel decodedModel = JsonConvert.DeserializeObject<SmartHealthCardModel>(SmartHealthCard);
                List<SmartHealthCardVaccineInfo> vaccineInfo = await _codingService.GetShcVaccineInfo(
                    decodedModel.VerifiableCredential.CredentialSubject.Immunizations);
                Debug.Print("Smart health card decoded with " +
                    $"{decodedModel.VerifiableCredential.CredentialSubject.Immunizations.Count()} immunizations " +
                    $"and {decodedModel.VerifiableCredential.CredentialSubject.Observations.Count()} observations.");

                // Step 8. Wrap and verify token result
                SmartHealthCardWrapper wrapper = new SmartHealthCardWrapper(decodedModel, vaccineInfo, issuer);
                resultModel.DecodedModel = wrapper;
                resultModel.ValidationResult = wrapper.Validate();
                return resultModel;
            }
            catch (Exception e)
            {
                // If any exception is thrown - assume the code is invalid.
                Debug.Print(">> Exception thrown from decoding: " + e.Message + ">>>>" + e.StackTrace);
                if (!_connectivityService.HasInternetConnection())
                {
                    resultModel.ValidationResult = TokenValidateResult.NoInternet;
                }
                return resultModel;
            }
        }

        private string DecodeSHCJwsToken(string qrCode)
        {
            // Decode QR -> Numeric -> JWS Token
            StringBuilder StringBuilder = new StringBuilder();
            string unprefixed = qrCode.Substring(5); // delete shc:/
            foreach (string Number in Spliter(unprefixed, 2))
            {
                if (int.TryParse(Number, out int IntNumber))
                {
                    StringBuilder.Append(Convert.ToChar(IntNumber + 45));
                }
            }

            string jwsToken = StringBuilder.ToString();
            Debug.Print($"{nameof(HcertTokenProcessorService)}: Decoded JWS token - {jwsToken}");
            if (string.IsNullOrEmpty(jwsToken))
            {
                throw new InvalidDataException("jws cannot be empty");
            }

            return jwsToken;
        }

        private IEnumerable<string> Spliter(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
