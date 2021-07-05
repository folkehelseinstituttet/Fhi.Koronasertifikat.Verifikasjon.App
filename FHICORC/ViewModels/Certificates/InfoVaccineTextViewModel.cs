using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.Base;

namespace FHICORC.ViewModels.Certificates
{
    public class InfoVaccineTextViewModel : BaseScanViewModel
    {
        #region Bindable properties

        public string VaccineHeaderText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_HEADER_TEXT".Translate() : "INFO_VACCINE_HEADER_TEXT".Translate();
        public string VaccineHeaderTextAccessibility => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_HEADER_TEXT_ACCESSIBILITY".Translate() : "INFO_VACCINE_HEADER_TEXT_ACCESSIBILITY".Translate();

        public string VaccineMarketingAuthorizationText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_MARKETING_AUTHORISATION_HOLDER_TEXT".Translate() : "INFO_VACCINE_MARKETING_AUTHORISATION_HOLDER_TEXT".Translate();
        public string VaccineMarketingAuthorizationText_Accessibility => VaccineMarketingAuthorizationText.ToLower();

        public string VaccineDoseTitleText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_DOSE_TITLE_TEXT".Translate() : "INFO_VACCINE_DOSE_ONE_TEXT".Translate();
        public string CurrentDoseTemplate => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_CURRENT_DOSE_TEXT".Translate() : "INFO_VACCINE_CURRENT_DOSE_TEXT".Translate();
        public string VaccineTypeText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_TYPE_TEXT".Translate() : "INFO_VACCINE_TYPE_TEXT".Translate();
        public string VaccineVaccineNumberText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_VACCINE_NUMBER_TEXT".Translate() : "INFO_VACCINE_VACCINE_NUMBER_TEXT".Translate();

        public string VaccineVaccineNameText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_VACCINE_NAME_TEXT".Translate() : "INFO_VACCINE_VACCINE_NAME_TEXT".Translate();
        public string VaccineVaccineNameText_Accessibility => VaccineVaccineNameText.ToLower();

        public string VaccineVaccinationCountryText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_VACCINATION_COUNTRY_TEXT".Translate() : "INFO_VACCINE_VACCINATION_COUNTRY_TEXT".Translate();
        public string VaccineVaccinationCountryText_Accessibility => VaccineVaccinationCountryText.ToLower();


        public string VaccineAdministeringCenterText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_ADMINISTERING_CNETRE_NAME_TEXT".Translate() : "INFO_VACCINE_ADMINISTERING_CNETRE_NAME_TEXT".Translate();
        public string VaccineCertificateHeaderText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_HEADER_TEXT".Translate() : "INFO_CERTIFICATE_HEADER_TEXT".Translate();

        public string VaccineCertificateIssuerText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_ISSUER_TEXT".Translate() : "INFO_CERTIFICATE_ISSUER_TEXT".Translate();
        public string VaccineCertificateIssuerText_Accessibility => VaccineCertificateIssuerText.ToLower();


        public string VaccinePassportNumberText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate() : "INFO_CERTIFICATE_PASSPORT_NUMBER_TEXT".Translate();
        public string VaccinePassportNumberText_Accessibility => VaccinePassportNumberText.ToLower();

        public string VaccineCertificateSchemaText => ShowTextInEnglish ? "INTERNATIONAL_INFO_CERTIFICATE_SCHEMA_VERSION".Translate() : "INFO_CERTIFICATE_SCHEMA_VERSION".Translate();

        public string VaccineDateOfVaccinationText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_DATE_OF_VACCINATION_TEXT".Translate() : "INFO_VACCINE_DATE_OF_VACCINATION_TEXT".Translate();
        public string VaccineDateOfVaccinationText_Accessibility => VaccineDateOfVaccinationText.ToLower();

        public string VaccineTargetedDiseaseText => ShowTextInEnglish ? "INTERNATIONAL_INFO_VACCINE_DISEASE_TEXT".Translate() : "INFO_VACCINE_DISEASE_TEXT".Translate();
        public string VaccineTargetedDiseaseText_Accessibility => VaccineTargetedDiseaseText.ToLower();

        #endregion

        public string VaccineMarketingAuthorizationValue => _translator.Translate(DGCValueSetEnum.VaccineAuthorityHolder, PassportViewModel.PassportData.MarketingAuthorizationHolder ?? "").ToString();
        public string VaccineCurrentDoseValue => string.Format(CurrentDoseTemplate,
            PassportViewModel.PassportData.DoseNumber,
            PassportViewModel.PassportData.TotalNumberOfDose);
        public string VaccineTypeValue => _translator.Translate(DGCValueSetEnum.VaccineProphylaxis, PassportViewModel.PassportData.VaccinationType ?? "").ToString();
        public string VaccineVaccineNameValue => _translator.Translate(DGCValueSetEnum.VaccineMedicinalProduct, PassportViewModel.PassportData.MedicinialProduct ?? "").ToString();
        public string VaccineVaccinationCountryValue => PassportViewModel.PassportData.VaccinationCountry;
        public string VaccineCertificateIssuerValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIssuer : "-";
        public string VaccinePassportNumberValue => ShowCertificate ? PassportViewModel.PassportData.CertificateIdentifier : "-";
        public string VaccineCertificateSchemaValue => ShowCertificate ? PassportViewModel.PassportData.CertificateSchemaVersion : "-";
        public string VaccineHeaderValue => ShowHeader ? _translator.Translate(DGCValueSetEnum.VaccineAuthorityHolder, PassportViewModel.PassportData.MarketingAuthorizationHolder ?? "").ToString() : "-";
        public string VaccineVaccinationDateValue => PassportViewModel.PassportData.VaccinationDate;
        public string VaccineTargetDisease => _translator.Translate(DGCValueSetEnum.Disease, PassportViewModel.PassportData.Disease ?? "").ToString();

        public PassportViewModel PassportViewModel { get; set; } = new PassportViewModel();

        public bool ShowCertificate { get; set; } = true;
        public bool ShowHeader { get; set; }
        public bool ShowTextInEnglish { get; set; }

        private IDgcValueSetTranslator _translator;

        public InfoVaccineTextViewModel(ITimer timer) : base(timer)
        {
            _translator = DigitalGreenValueSetTranslatorFactory.DgcValueSetTranslator;
        }


        public void UpdateView()
        {
            OnPropertyChanged(nameof(PassportViewModel));
            OnPropertyChanged(nameof(ShowCertificate));
            OnPropertyChanged(nameof(ShowHeader));
            if (PassportViewModel == null) return;
            OnPropertyChanged(nameof(VaccineMarketingAuthorizationValue));
            OnPropertyChanged(nameof(VaccineCurrentDoseValue));
            OnPropertyChanged(nameof(VaccineTypeValue));
            OnPropertyChanged(nameof(VaccineVaccineNameValue));
            //OnPropertyChanged(nameof(VaccineVaccineNumberValue));
            OnPropertyChanged(nameof(VaccineVaccinationCountryValue));
            OnPropertyChanged(nameof(VaccineVaccinationDateValue));
            OnPropertyChanged(nameof(VaccineTargetDisease));

            if (ShowCertificate)
            {
                OnPropertyChanged(nameof(VaccineCertificateIssuerValue));
                OnPropertyChanged(nameof(VaccinePassportNumberValue));
                OnPropertyChanged(nameof(VaccineCertificateSchemaValue));
            }
            if (ShowHeader)
            {
                OnPropertyChanged(nameof(VaccineHeaderValue));
            }

        }
    }
}
