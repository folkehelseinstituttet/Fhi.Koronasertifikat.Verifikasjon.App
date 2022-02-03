using FHICORC.Configuration;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.ScannerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanSHCVaccineResultView : ContentPage, IScanResultView
    {
        public ScanSHCVaccineResultView()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ScanSHCVaccineResultViewModel>();
        }

        protected override void OnDisappearing()
        {
            ((ScanSHCVaccineResultViewModel)BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            ((ScanSHCVaccineResultViewModel)BindingContext).Timer.Enabled = true;
            base.OnAppearing();
        }
    }
}
