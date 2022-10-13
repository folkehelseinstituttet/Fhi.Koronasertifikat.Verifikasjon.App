using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Model;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScannerErrorViewModel : BaseScanViewModel
    {
        public string PageTitle => TokenValidateResultModel.ValidationResult switch
        {
            TokenValidateResult.Invalid => "SCANNER_ERROR_INVALID_TITLE".Translate(),
            TokenValidateResult.Expired => "SCANNER_ERROR_EXPIRED_TITLE".Translate(),
            TokenValidateResult.UnsupportedType => "SCANNER_ERROR_DEFAULT_TITLE".Translate(),
            TokenValidateResult.Revoked => "SCANNER_ERROR_REVOKED_TITLE".Translate(),
            _ => "SCANNER_ERROR_DEFAULT_TITLE".Translate()
        }; 
        public string ContentText => TokenValidateResultModel.ValidationResult switch
        {
            TokenValidateResult.Invalid => "SCANNER_ERROR_INVALID_CONTENT".Translate(),
            TokenValidateResult.Expired => "SCANNER_ERROR_EXPIRED_CONTENT".Translate(),
            TokenValidateResult.UnsupportedType => "SCANNER_ERROR_UNKNOWN_TYPE_CONTENT".Translate(),
            TokenValidateResult.Revoked => "SCANNER_ERROR_REVOKED_CONTENT_REV".Translate(),
            _ => "SCANNER_ERROR_DEFAULT_CONTENT_REV".Translate(),
        };

        public TokenValidateResultModel TokenValidateResultModel { get; set; } = new TokenValidateResultModel();

        public string RepeatedText => string.Concat(Enumerable.Repeat(PageTitle.PadLeft(12), 10));
        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ScannerErrorViewModel(ITimer timer) : base(timer) { }

        public override Task InitializeAsync(object navigationData)
        {
            if (!(navigationData is TokenValidateResultModel tokenValidateResultModel))
                return base.InitializeAsync(navigationData);

            TokenValidateResultModel = tokenValidateResultModel;

            OnPropertyChanged(nameof(TokenValidateResultModel));
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(RepeatedText));
            OnPropertyChanged(nameof(ContentText));

            return base.InitializeAsync(navigationData);
        }
    }
}