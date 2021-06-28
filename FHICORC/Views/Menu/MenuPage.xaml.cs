using System;
using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels;
using FHICORC.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage(MenuPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);
        }
    }
}