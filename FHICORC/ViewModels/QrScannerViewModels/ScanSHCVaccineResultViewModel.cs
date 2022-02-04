using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Model;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Data;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;
using FHICORC.Utils;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScanSHCVaccineResultViewModel : BaseScanViewModel
    {
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _dateOfBirth;
        public string DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        private string _vaccinationDate;
        public string VaccinationDate
        {
            get => _vaccinationDate;
            set
            {
                _vaccinationDate = value;
                OnPropertyChanged(nameof(VaccinationDate));
            }
        }

        private string _vaccineName;
        public string VaccineName
        {
            get => _vaccineName;
            set
            {
                _vaccineName = value;
                OnPropertyChanged(nameof(VaccineName));
            }
        }

        private string _vaccineType;
        public string VaccineType
        {
            get => _vaccineType;
            set
            {
                _vaccineType = value;
                OnPropertyChanged(nameof(VaccineType));
            }
        }

        private string _vaccineManufacturer;
        public string VaccineManufacturer
        {
            get => _vaccineManufacturer;
            set
            {
                _vaccineManufacturer = value;
                OnPropertyChanged(nameof(VaccineManufacturer));
            }
        }

        private string _vaccineNumberOfDoses;
        public string VaccineNumberOfDoses
        {
            get => _vaccineNumberOfDoses;
            set
            {
                _vaccineNumberOfDoses = value;
                OnPropertyChanged(nameof(VaccineNumberOfDoses));
            }
        }

        private string _vaccineTargetDisease;
        public string VaccineTargetDisease
        {
            get => _vaccineTargetDisease;
            set
            {
                _vaccineTargetDisease = value;
                OnPropertyChanged(nameof(VaccineTargetDisease));
            }
        }

        private string _issuerName;
        public string IssuerName
        {
            get => _issuerName;
            set
            {
                _issuerName = value;
                OnPropertyChanged(nameof(IssuerName));
            }
        }

        private string _bannerText;
        public string BannerText
        {
            get => _bannerText;
            set
            {
                _bannerText = value;
                OnPropertyChanged(nameof(BannerText));
            }
        }

        private Color _bannerColor;
        public Color BannerColor
        {
            get => _bannerColor;
            set
            {
                _bannerColor = value;
                OnPropertyChanged(nameof(BannerColor));
            }
        }

        private Color _bannerTextColor;
        public Color BannerTextColor
        {
            get => _bannerTextColor;
            set
            {
                _bannerTextColor = value;
                OnPropertyChanged(nameof(BannerTextColor));
            }
        }

        private readonly IPreferencesService _preferencesService;
        public bool IsBorderControlOn => _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.BORDER_CONTROL_ON);

        public string RepeatedText => string.Concat(Enumerable.Repeat($"{BannerText}         ", 10));

        public string VaccineHeaderText => "INTERNATIONAL_INFO_VACCINE_HEADER_TEXT".Translate();
        public string VaccineStatusText => "SHC_VACCINE_STATUS_TEXT".Translate();
        public string VaccineDateOfVaccinationText => "INTERNATIONAL_INFO_VACCINE_DATE_OF_VACCINATION_TEXT".Translate();
        public string VaccineNameText => "INTERNATIONAL_INFO_VACCINE_VACCINE_NAME_TEXT".Translate();
        public string VaccineTypeText => "INTERNATIONAL_INFO_VACCINE_TYPE_TEXT".Translate();
        public string VaccineManufacturerText =>  "INTERNATIONAL_INFO_VACCINE_MARKETING_AUTHORISATION_HOLDER_TEXT".Translate();
        public string VaccineNumberOfDosesText => "INTERNATIONAL_INFO_VACCINE_DOSE_TITLE_TEXT".Translate();
        public string VaccineTargetedDiseaseText => "INTERNATIONAL_INFO_VACCINE_DISEASE_TEXT".Translate();
        public string VaccineCertificateHeaderText => "INTERNATIONAL_INFO_CERTIFICATE_HEADER_TEXT".Translate();
        public string VaccineCertificateIssuerText => "INTERNATIONAL_INFO_CERTIFICATE_ISSUER_TEXT".Translate();

        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));


        public ScanSHCVaccineResultViewModel(ITimer timer, IPreferencesService preferencesService) : base(timer)
        {
            _preferencesService = preferencesService;
            CheckControlType();
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                if (navigationData is TokenValidateResultModel tokenValidateResultModel)
                {
                    if (tokenValidateResultModel.DecodedModel is SmartHealthCardWrapper shc)
                    {
                        // Assumes one patient and one type if vaccine/immunization
                        SmartHealthCardPatient patient = shc.SmartHealthCard.VerifiableCredential.CredentialSubject.Patients.First();
                        SmartHealthCardImmunization immunization = shc.SmartHealthCard.VerifiableCredential.CredentialSubject.Immunizations.OrderByDescending(x => x.OccurrenceDateTime).First();
                        SmartHealthCardVaccineInfo vaccineInfo = shc.VaccineInfo.First(x => x.Id.Equals(immunization.VaccineCode.Id));
                        SmartHealthCardIssuer issuer = shc.SmartHealthCardIssuer;

                        FullName = patient.PersonName.FullName;
                        DateOfBirth = patient.DateOfBirth;
                        VaccinationDate = immunization.OccurrenceDateTime?.ToLocaleDateFormat();
                        VaccineName = vaccineInfo.Name;
                        VaccineType = vaccineInfo.Type;
                        VaccineManufacturer = vaccineInfo.Manufacturer;
                        VaccineNumberOfDoses = shc.SmartHealthCard.VerifiableCredential.CredentialSubject.Immunizations.Count().ToString();
                        VaccineTargetDisease = vaccineInfo.Target;
                        IssuerName = issuer.Name;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to initialise TestResult scan: {e}");
            }

            return base.InitializeAsync(navigationData);
        }

        private void CheckControlType()
        {
            if (IsBorderControlOn)
            {
                BannerColor = Color.FromHex("#32345C");
                BannerTextColor = Color.FromHex("#F3F9FB");
                BannerText = "SCANNER_EU_BANNER_TEXT".Translate();
            }
            else
            {
                BannerColor = Color.FromHex("F3F9FB");
                BannerTextColor = Color.FromHex("#32345C");
                BannerText = "SCANNER_NO_BANNER_TEXT".Translate();
            }
        }
    }
}
