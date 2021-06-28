using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.Services.Model;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;

namespace FHICORC.Views.ScannerPages
{
    public partial class ScannerErrorPage : ContentPage, IScanResultView
    {
        public ScannerErrorPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ScannerErrorViewModel>();
        }

        protected override void OnDisappearing()
        {
            ((ScannerErrorViewModel)BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }
    }
}
