using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.Base;

namespace FHICORC.ViewModels.Certificates
{
    public class InfoTestTextViewModel : BaseScanViewModel
    {
        #region Bindable properties

        public string NegativeTestHeaderText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_HEADER_TEXT".Translate() : "NEGATIVE_TEST_HEADER_TEXT".Translate();

        public string NegativeTestHeaderTextAccessibility => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_HEADER_TEXT_ACCESSIBILITY".Translate() : "NEGATIVE_TEST_HEADER_TEXT_ACCESSIBILITY".Translate();


        public string NegativeTestTypeOfTestText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_TYPE_OF_TEST_TEXT".Translate() : "NEGATIVE_TEST_TYPE_OF_TEST_TEXT".Translate();
        public string NegativeTestTypeOfTestText_Accessibility => NegativeTestTypeOfTestText.ToLower();

        public string NegativeTestTestNameText => "INTERNATIONAL_NEGATIVE_TEST_TEST_NAME_TEXT".Translate();
        public string NegativeTestTestNameText_Accessibility => NegativeTestTestNameText.ToLower();

        public string NegativeTestNAATestNameText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_NAA_TEST_NAME".Translate() : "NEGATIVE_TEST_NAA_TEST_NAME".Translate();
        public string NegativeTestNAATestNameText_Accessibility => NegativeTestNAATestNameText.ToLower();

        public string NegativeTestSampleOriginText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_SAMPLE_OF_ORIGIN_TEXT".Translate() : "NEGATIVE_TEST_SAMPLE_OF_ORIGIN_TEXT".Translate();
        public string NegativeTestSampleOriginText_Accessibility => NegativeTestSampleOriginText.ToLower();

        public string NegativeTestSampleCollectedDateTimeText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_SAMPLE_COLLECTED_DATETIME".Translate() : "NEGATIVE_TEST_SAMPLE_COLLECTED_DATETIME".Translate();
        public string NegativeTestSampleCollectedDateTimeText_Accessibility => NegativeTestSampleCollectedDateTimeText.ToLower();

        public string NegativeTestTestCenterTimeText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_TEST_CENTER".Translate() : "NEGATIVE_TEST_TEST_CENTER".Translate();
        public string NegativeTestTestCenterTimeText_Accessibility => NegativeTestTestCenterTimeText.ToLower();

        public string NegativeTestResultText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_RESULT_TEXT".Translate() : "NEGATIVE_TEST_RESULT_TEXT".Translate();
        public string NegativeTestResultText_Accessibility => NegativeTestResultText.ToLower();

        public string NegativeTestResultProductionDateTimeText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_RESULT_PRODUCTION_DATETIME_TEXT".Translate() : "NEGATIVE_TEST_RESULT_PRODUCTION_DATETIME_TEXT".Translate();
        public string NegativeTestResultProductionDateTimeText_Accessibility => NegativeTestResultProductionDateTimeText.ToLower();

        public string NegativeTestDiseaseText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_DISEASE_TEXT".Translate() : "NEGATIVE_TEST_DISEASE_TEXT".Translate();
        public string NegativeTestDiseaseText_Accessibility => NegativeTestDiseaseText.ToLower();

        public string NegativeTestManufacturerText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_MANUFACTURER_TEXT".Translate() : "NEGATIVE_TEST_MANUFACTURER_TEXT".Translate();
        public string NegativeTestManufacturerText_Accessibility => NegativeTestManufacturerText.ToLower();

        public string NegativeTestTestingCountryText => ShowTextInEnglish ? "INTERNATIONAL_NEGATIVE_TEST_TESTING_COUNTRY_TEXT".Translate() : "NEGATIVE_TEST_TESTING_COUNTRY_TEXT".Translate();
        public string NegativeTestTestingCountryText_Accessibility => NegativeTestTestingCountryText.ToLower();

        public string NegativeTestCertificateHeaderText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_HEADER_TEXT".Translate() : "INFO_CERTIFICATE_HEADER_TEXT".Translate();
        public string NegativeTestCertificateHeaderText_Accessibility => NegativeTestCertificateHeaderText.ToLower();

        public string NegativeTestIssuerText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_ISSUER_TEXT".Translate() : "INFO_CERTIFICATE_ISSUER_TEXT".Translate();
        public string NegativeTestIssuerText_Accessibility => NegativeTestIssuerText.ToLower();

        public string NegativeTestCertificatIdentifierText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate() : "INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate();
        public string NegativeTestCertificatIdentifierText_Accessibility => NegativeTestCertificatIdentifierText.ToLower();

        #endregion

        public string NegativeTestTypeOfTestValue => _translator.Translate(DGCValueSetEnum.TypeOfTest, PassportViewModel.PassportData.TypeOfTest ?? "").ToString();
        public string NegativeTestResultValue => _translator.Translate(DGCValueSetEnum.TestResult, PassportViewModel.PassportData.Result ?? "").ToString();
        public string NegativeTestSampleCollectionDateTimeValue => PassportViewModel.PassportData.SampleCollectedTime?.ToLocaleDateTimeFormat() ?? "-";
        public string NegativeTestDiseaseValue => _translator.Translate(DGCValueSetEnum.Disease, PassportViewModel.PassportData.Disease ?? "").ToString();
        public string NegativeTestManufacturerValue => _translator.Translate(DGCValueSetEnum.TestManufacturer, PassportViewModel.PassportData.TestManufacturer ?? "").ToString();
        public string NegativeTestNumberValue => "";
        public string NegativeTestSampleOriginValue => PassportViewModel.PassportData.SampleOrigin;
        public string NegativeTestTestingCentreValue => PassportViewModel.PassportData.TestCountry ?? "-";
        public string NegativeTestIssuerValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIssuer : "-";
        public string NegativeTestCertificatIdentifierValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIdentifier : "-";
        public string TestHeaderValue => ShowHeader ? _translator.Translate(DGCValueSetEnum.TypeOfTest, PassportViewModel.PassportData.TypeOfTest ?? "").ToString() : "-";
        public string TestCenter => PassportViewModel.PassportData.TestCenter;
        public string TestName => string.IsNullOrEmpty(PassportViewModel.PassportData.TestName) ? "-" : _translator.Translate(DGCValueSetEnum.TypeOfTest, PassportViewModel.PassportData.TestName ?? "").ToString();
        public string NAATestName => PassportViewModel.PassportData.NAATestName;

        public PassportViewModel PassportViewModel { get; set; } = new PassportViewModel();

        public bool ShowCertificate { get; set; } = true;
        public bool ShowHeader { get; set; }
        public bool ShowTextInEnglish { get; set; }

        private IDgcValueSetTranslator _translator;

        public InfoTestTextViewModel(ITimer timer) : base(timer)
        {
            _translator = DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
        }

        public void UpdateView()
        {
            OnPropertyChanged(nameof(NegativeTestHeaderText));
            OnPropertyChanged(nameof(NegativeTestHeaderTextAccessibility));
            OnPropertyChanged(nameof(NegativeTestTypeOfTestText));
            OnPropertyChanged(nameof(NegativeTestTestNameText));
            OnPropertyChanged(nameof(NegativeTestSampleCollectedDateTimeText));
            OnPropertyChanged(nameof(NegativeTestTestCenterTimeText));
            OnPropertyChanged(nameof(NegativeTestResultProductionDateTimeText));
            OnPropertyChanged(nameof(NegativeTestDiseaseText));
            OnPropertyChanged(nameof(NegativeTestManufacturerText));
            OnPropertyChanged(nameof(NegativeTestTestingCountryText));
            OnPropertyChanged(nameof(NegativeTestCertificateHeaderText));
            OnPropertyChanged(nameof(NegativeTestIssuerText));
            OnPropertyChanged(nameof(NegativeTestCertificatIdentifierText));
            OnPropertyChanged(nameof(NegativeTestResultText));

            OnPropertyChanged(nameof(ShowCertificate));
            OnPropertyChanged(nameof(PassportViewModel));
            OnPropertyChanged(nameof(ShowHeader));

            if (PassportViewModel == null) return;

            OnPropertyChanged(nameof(NegativeTestTypeOfTestValue));
            OnPropertyChanged(nameof(NegativeTestResultValue));
            OnPropertyChanged(nameof(NegativeTestSampleCollectionDateTimeValue));
            OnPropertyChanged(nameof(NegativeTestDiseaseValue));
            OnPropertyChanged(nameof(NegativeTestManufacturerValue));
            OnPropertyChanged(nameof(NegativeTestSampleOriginValue));
            OnPropertyChanged(nameof(NegativeTestTestingCentreValue));
            OnPropertyChanged(nameof(TestCenter));
            OnPropertyChanged(nameof(TestName));
            OnPropertyChanged(nameof(NAATestName));

            if (ShowCertificate)
            {
                OnPropertyChanged(nameof(NegativeTestIssuerValue));
                OnPropertyChanged(nameof(NegativeTestCertificatIdentifierValue));
            }
            if (ShowHeader)
            {
                OnPropertyChanged(nameof(TestHeaderValue));
            }

        }
    }
}
