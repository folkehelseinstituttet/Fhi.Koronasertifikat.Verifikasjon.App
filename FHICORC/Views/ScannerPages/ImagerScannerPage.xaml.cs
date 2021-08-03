using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.ScannerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagerScannerPage : ContentPage
    {
        public ImagerScannerPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ImagerScannerViewModel>();
        }

        protected override void OnAppearing()
        {
            ((ImagerScannerViewModel)BindingContext).EnableScanner();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((ImagerScannerViewModel)BindingContext).DisableScanner();

            base.OnDisappearing();
        }
    }
}