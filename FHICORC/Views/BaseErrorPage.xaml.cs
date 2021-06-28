using FHICORC.Configuration;
using FHICORC.Enums;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.Error;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseErrorPage : ContentPage
    {
        public BaseErrorPage(ErrorPageType type = ErrorPageType.Default)
        {
            InitializeComponent();
            switch (type) {
                case ErrorPageType.ForceUpdate:
                    BindingContext = IoCContainer.Resolve<ForceUpdateViewModel>();
                    break;
                case ErrorPageType.NoInternet:
                    BindingContext = IoCContainer.Resolve<NoInternetViewModel>();
                    break;
                default:
                    BindingContext = IoCContainer.Resolve<BaseErrorViewModel>();
                    break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.DefaultBackgroundColor.Color(), Color.Black);
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}