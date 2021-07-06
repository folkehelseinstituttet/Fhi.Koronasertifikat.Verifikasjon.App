using System;
using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<LandingViewModel>();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.LandingPageColorStart.Color(), Color.White);

        }

        void TapGestureRecognizer_Tapped_Border(object sender, EventArgs e)
        {
            if (!RadioButtonBorder.IsChecked)
            {
                RadioButtonBorder.IsChecked = true;
            }
        }

        void TapGestureRecognizer_Tapped_Domestic(object sender, EventArgs e)
        {
            if (!RadioButtonDomestic.IsChecked)
            {
                RadioButtonDomestic.IsChecked = true;
            }
        }
    }
}