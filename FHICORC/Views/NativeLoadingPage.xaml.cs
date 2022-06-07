using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NativeLoadingPage : ContentPage
    {
        public NativeLoadingPage()
        {
            InitializeComponent();
            BindingContext = new NativeLoadingViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), FHICORCColor.FHIPrimaryBlue.Color());
        }
    }
}