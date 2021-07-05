using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.Base;

namespace FHICORC.ViewModels.Certificates
{
    public class InfoRecoveryTextViewModel : BaseScanViewModel
    {
        #region Bindable properties

        public string RecoveryHeaderText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_HEADER_TEXT".Translate() : "RECOVERY_HEADER_TEXT".Translate();
        public string RecoveryHeaderText_Accessibility => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_HEADER_TEXT_ACCESSIBILITY".Translate() : "INFO_VACCINE_HEADER_TEXT_ACCESSIBILITY".Translate();

        public string RecoveryDiseaseText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_DISEASE_TEXT".Translate() : "RECOVERY_DISEASE_TEXT".Translate();
        public string RecoveryDiseaseText_Accessibility => RecoveryDiseaseText.ToLower();

        public string RecoveryDateText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_DATE_TEXT".Translate() : "RECOVERY_DATE_TEXT".Translate();
        public string RecoveryDateText_Accessibility => RecoveryDateText.ToLower();


        public string RecoveryCountryText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_COUNRTY_TEXT".Translate() : "RECOVERY_COUNRTY_TEXT".Translate();
        public string RecoveryCountryText_Accessibility => RecoveryCountryText.ToLower();

        public string RecoveryCertificateHeaderText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_CERTIFICATE_HEADER_TEXT".Translate() : "RECOVERY_CERTIFICATE_HEADER_TEXT".Translate();
        public string RecoveryCertificateHeaderText_Accessibility => RecoveryCertificateHeaderText.ToLower();

        public string RecoveryIdentifierText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate() : "INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate();
        public string RecoveryIdentifierText_Accessibility => RecoveryIdentifierText.ToLower();

        public string RecoveryIssuerText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_CERTIFICATE_ISSUER_TEXT".Translate() : "RECOVERY_CERTIFICATE_ISSUER_TEXT".Translate();
        public string RecoveryIssuerText_Accessibility => RecoveryIssuerText.ToLower();

        public string RecoveryValidFromText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_VALID_FROM_TEXT".Translate() : "RECOVERY_VALID_FROM_TEXT".Translate();
        public string RecoveryValidFromText_Accessibility => RecoveryValidFromText.ToLower();

        public string RecoveryValidToText => ShowTextInEnglish ? "INTERNATIONAL_RECOVERY_VALID_TO_TEXT".Translate() : "RECOVERY_VALID_TO_TEXT".Translate();
        public string RecoveryValidToText_Accessibility => RecoveryValidToText.ToLower();

        #endregion

        public string RecoveryDiseaseValue => _translator.Translate(DGCValueSetEnum.Disease, PassportViewModel.PassportData.RecoveryDisease ?? "").ToString();
        public string RecoveryDateValue => PassportViewModel.PassportData.DateFirstPositiveTest ?? "-";
        public string RecoveryCountryValue => PassportViewModel.PassportData.CountryOfTest;
        public string RecoveryIssuerValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIssuer : "-";
        public string RecoveryValidFromValue => ShowCertificate ? PassportViewModel.PassportData.CertificateValidFrom?.ToLocaleDateFormat() : "-";
        public string RecoveryValidToValue => ShowCertificate ? PassportViewModel.PassportData.CertificateValidTo?.ToLocaleDateFormat() : "-";
        public string RecoveryHeaderValue => ShowHeader ? _translator.Translate(DGCValueSetEnum.Disease, PassportViewModel.PassportData.RecoveryDisease ?? "").ToString() : "-";
        public string RecoveryIdentifierValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIdentifier : "-";

        public PassportViewModel PassportViewModel { get; set; } = new PassportViewModel();

        public bool ShowCertificate { get; set; } = true;
        public bool ShowHeader { get; set; }
        public bool ShowTextInEnglish { get; set; }

        private IDgcValueSetTranslator _translator;

        public InfoRecoveryTextViewModel(ITimer timer) : base(timer)
        {
            _translator = DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
        }

        public void UpdateView()
        {
            OnPropertyChanged(nameof(PassportViewModel));
            OnPropertyChanged(nameof(ShowCertificate));
            OnPropertyChanged(nameof(ShowHeader));
            OnPropertyChanged(nameof(ShowTextInEnglish));


            if (PassportViewModel == null) return;
            OnPropertyChanged(nameof(RecoveryDiseaseValue));
            OnPropertyChanged(nameof(RecoveryDateValue));
            OnPropertyChanged(nameof(RecoveryCountryValue));

            if (ShowCertificate)
            {
                OnPropertyChanged(nameof(RecoveryIssuerValue));
                OnPropertyChanged(nameof(RecoveryValidFromValue));
                OnPropertyChanged(nameof(RecoveryValidToValue));
                OnPropertyChanged(nameof(RecoveryIdentifierValue));
            }
            if (ShowHeader)
            {
                OnPropertyChanged(nameof(RecoveryHeaderValue));
            }

        }
    }
}
