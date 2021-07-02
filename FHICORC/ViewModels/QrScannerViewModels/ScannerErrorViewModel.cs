using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public string PageTitle => ShowInvalidPage ? "SCANNER_ERROR_INVALID_TITLE".Translate() : "SCANNER_ERROR_EXPIRED_TITLE".Translate();
        public string InvalidContentText => TokenValidateResultModel.ValidationResult == TokenValidateResult.Invalid ? "SCANNER_ERROR_INVALID_CONTENT".Translate()
            : "SCANNER_ERROR_UNKNOWN_TYPE_CONTENT".Translate();
        public bool ShowInvalidPage => TokenValidateResultModel.ValidationResult == TokenValidateResult.Invalid
            || TokenValidateResultModel.ValidationResult == TokenValidateResult.UnsupportedType;
        public TokenValidateResultModel TokenValidateResultModel { get; set; } = new TokenValidateResultModel();

        public string RepeatedText => string.Concat(Enumerable.Repeat(PageTitle.PadLeft(15), 10));
        public ICommand ScanAgainCommand => new Command(async () =>
            await ExecuteOnceAsync(async () => await Task.Run(ClosePage)));

        public ScannerErrorViewModel(ITimer timer) : base(timer) { }

        public override Task InitializeAsync(object navigationData)
        {
            if (!(navigationData is TokenValidateResultModel tokenValidateResultModel))
                return base.InitializeAsync(navigationData);

            TokenValidateResultModel = tokenValidateResultModel;

            OnPropertyChanged(nameof(TokenValidateResultModel));
            OnPropertyChanged(nameof(ShowInvalidPage));
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(RepeatedText));
            OnPropertyChanged(nameof(InvalidContentText));

            return base.InitializeAsync(navigationData);
        }
    }
}