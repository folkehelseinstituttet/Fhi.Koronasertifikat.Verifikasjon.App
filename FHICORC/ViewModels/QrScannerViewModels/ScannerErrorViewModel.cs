using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Core.Data;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScannerErrorViewModel : BaseScanViewModel
    {
        public TokenValidateResultModel TokenValidateResultModel { get; set; } = new TokenValidateResultModel();

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }

        private string _invalidContentTitle;
        public string InvalidContentTitle
        {
            get => _invalidContentTitle;
            set
            {
                _invalidContentTitle = value;
                OnPropertyChanged(nameof(InvalidContentTitle));
            }
        }

        private string _invalidContentText;
        public string InvalidContentText
        {
            get => _invalidContentText;
            set
            {
                _invalidContentText = value;
                OnPropertyChanged(nameof(InvalidContentText));
            }
        }

        private string _repeatedText;
        public string RepeatedText
        {
            get => _repeatedText;
            set
            {
                _repeatedText = value;
                OnPropertyChanged(nameof(RepeatedText));
            }
        }

        private bool _showInvalidPage = false;
        public bool ShowInvalidPage
        {
            get => _showInvalidPage;
            set
            {
                _showInvalidPage = value;
                OnPropertyChanged(nameof(ShowInvalidPage));
            }
        }

        private bool _showExpiredPage = false;
        public bool ShowExpiredPage
        {
            get => _showExpiredPage;
            set
            {
                _showExpiredPage = value;
                OnPropertyChanged(nameof(ShowExpiredPage));
            }
        }

        private bool _showNoInternetPage = false;
        public bool ShowNoInternetPage
        {
            get => _showNoInternetPage;
            set
            {
                _showNoInternetPage = value;
                OnPropertyChanged(nameof(ShowNoInternetPage));
            }
        }

        private readonly IPreferencesService _preferencesService;
        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ScannerErrorViewModel(ITimer timer, IPreferencesService preferencesService) : base(timer)
        {
            _preferencesService = preferencesService;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (!(navigationData is TokenValidateResultModel tokenValidateResultModel))
                return base.InitializeAsync(navigationData);

            TokenValidateResultModel = tokenValidateResultModel;

            UpdateErrorScreenUI();

            OnPropertyChanged(nameof(TokenValidateResultModel));
            OnPropertyChanged(nameof(ShowInvalidPage));
            OnPropertyChanged(nameof(ShowExpiredPage));
            OnPropertyChanged(nameof(ShowNoInternetPage));
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(RepeatedText));
            OnPropertyChanged(nameof(InvalidContentTitle));
            OnPropertyChanged(nameof(InvalidContentText));

            return base.InitializeAsync(navigationData);
        }

        public void UpdateErrorScreenUI()
        {
            if (TokenValidateResultModel.ValidationResult == TokenValidateResult.Expired)
            {
                PageTitle = "SCANNER_ERROR_EXPIRED_PAGE_TITLE".Translate();
                InvalidContentTitle = "SCANNER_ERROR_EXPIRED_TITLE".Translate();
                if (_preferencesService.GetUserPreferenceAsBoolean("BORDER_CONTROL_ON"))
                {
                    InvalidContentText = "SCANNER_ERROR_EXPIRED_CONTENT_BORDER_CONTROL".Translate();
                }
                else
                {
                    InvalidContentText = "SCANNER_ERROR_EXPIRED_CONTENT".Translate();
                }
                RepeatedText = string.Concat(Enumerable.Repeat(PageTitle.PadLeft(12), 10));
                ShowExpiredPage = true;
            }
            else if (TokenValidateResultModel.ValidationResult == TokenValidateResult.NoInternet)
            {
                PageTitle = "SCANNER_ERROR_NO_INTERNET_TITLE".Translate();
                InvalidContentTitle = "SCANNER_ERROR_NO_INTERNET_TITLE".Translate();
                InvalidContentText = "SCANNER_ERROR_NO_INTERNET_CONTENT".Translate();
                RepeatedText = string.Concat(Enumerable.Repeat(PageTitle.PadLeft(20), 4));
                ShowNoInternetPage = true;
            }
            else if (TokenValidateResultModel.ValidationResult == TokenValidateResult.UnsupportedType)
            {
                PageTitle = "SCANNER_ERROR_INVALID_TITLE".Translate();
                InvalidContentTitle = "SCANNER_ERROR_INVALID_TITLE".Translate();
                InvalidContentText = "SCANNER_ERROR_UNKNOWN_TYPE_CONTENT".Translate();
                RepeatedText = string.Concat(Enumerable.Repeat(PageTitle.PadLeft(12), 10));
                ShowInvalidPage = true;
            }
            else
            {
                PageTitle = "SCANNER_ERROR_INVALID_TITLE".Translate();
                InvalidContentTitle = "SCANNER_ERROR_INVALID_TITLE".Translate();
                InvalidContentText = "SCANNER_ERROR_INVALID_CONTENT".Translate();
                RepeatedText = string.Concat(Enumerable.Repeat(PageTitle.PadLeft(12), 10));
                ShowInvalidPage = true;
            }
        } 
    }
}