using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<AboutPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);
        }
    }
}