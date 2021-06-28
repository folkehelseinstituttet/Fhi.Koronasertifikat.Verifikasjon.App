using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Data;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using FHICORC.Views;
using FHICORC.Views.ScannerPages;
using Xamarin.Forms;

namespace FHICORC.ViewModels
{
    public class AcceptTermsViewModel : BaseViewModel
    {

        public string AcceptTermsHtmlText { get; set; }
        public string AcceptTermsLabelText { get; set; }
        public ICommand NextButtonCommand { get; set; }

        private bool termsAccepted;
        public bool TermsAccepted
        {
            get
            {
                return termsAccepted;
            }
            set
            {
                termsAccepted = value;
                RaisePropertyChanged(() => TermsAccepted);
            }
        }

        private readonly IPreferencesService _preferencesService;
        private readonly IScannerFactory _scannerFactoryService;

        public AcceptTermsViewModel(IPreferencesService preferencesService, IScannerFactory scannerFactory)
        {
            TermsAccepted = false;
            _preferencesService = preferencesService;
            _scannerFactoryService = scannerFactory;
            AcceptTermsHtmlText = "ACCEPT_TERMS_HTML_TEXT".Translate();
            AcceptTermsLabelText = "ACCEPT_TERMS_LABEL_TEXT".Translate();
            NextButtonCommand = new Command(async() => await ExecuteOnceAsync(GoToScannerPage));
        }

        private async Task GoToScannerPage()
        {
            if (TermsAccepted)
            {
                _preferencesService.SetUserPreference(PreferencesKeys.TERMS_ACCEPTED, true);
                await _navigationService.PopPage();
                if (_scannerFactoryService.GetAvailableScanner() == null)
                {
                    await _navigationService.PushPage(new QRScannerPage());
                }
                else
                {
                    await _navigationService.PushPage(new ImagerScannerPage());
                }
            }
        }
    }
}
