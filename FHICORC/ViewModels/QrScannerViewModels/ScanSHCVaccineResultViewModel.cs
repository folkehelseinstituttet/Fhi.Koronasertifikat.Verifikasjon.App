using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Model;
using FHICORC.Data;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;

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

        private string _vaccineStatus;
        public string VaccineStatus
        {
            get => _vaccineStatus;
            set
            {
                _vaccineStatus = value;
                OnPropertyChanged(nameof(VaccineStatus));
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
                    if (tokenValidateResultModel.DecodedModel is Core.Services.Model.SmartHealthCardModel.Shc.SmartHealthCardModel shc)
                    {
                        FullName = shc.VerifiableCredential.CredentialSubject.Patients.First().PersonName.FullName;
                        DateOfBirth = shc.VerifiableCredential.CredentialSubject.Patients.First().DateOfBirth.ToString();
                        VaccinationDate = shc.VerifiableCredential.CredentialSubject.Immunizations.OrderByDescending(x => x.OccurrenceDateTime).First().OccurrenceDateTime.ToString();
                        VaccineStatus = shc.VerifiableCredential.CredentialSubject.Immunizations.OrderByDescending(x => x.OccurrenceDateTime).First().Status;
                        VaccineType = shc.VerifiableCredential.CredentialSubject.Immunizations.OrderByDescending(x => x.OccurrenceDateTime).First().VaccineCode.Coding[0].Code;
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
