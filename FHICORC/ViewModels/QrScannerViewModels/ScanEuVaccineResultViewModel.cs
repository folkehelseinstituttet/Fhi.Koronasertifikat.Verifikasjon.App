using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Models;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Certificates;
using Xamarin.Forms;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScanEuVaccineResultViewModel : InfoVaccineTextViewModel
    {
        private string _fullName;
        private string _dateOfBirth;

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        public string RepeatedText => string.Concat(Enumerable.Repeat($"{"SCANNER_EU_BANNER_TEXT".Translate()}         ", 10));

        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ScanEuVaccineResultViewModel(ITimer timer) : base(timer)
        {
            ShowTextInEnglish = true;
            ShowHeader = true;
        }

        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                if (navigationData is Core.Services.Model.EuDCCModel._1._3._0.DCCPayload cwt)
                {
                    FullName = cwt.DCCPayloadData.DCC.PersonName.FullName;
                    DateOfBirth = cwt.DCCPayloadData.DCC.DateOfBirth;
                    PassportViewModel.PassportData = new PassportData(string.Empty, cwt);
                    UpdateView();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to initialise TestResult scan: {e}");
            }

            return base.InitializeAsync(navigationData);
        }
    }
}