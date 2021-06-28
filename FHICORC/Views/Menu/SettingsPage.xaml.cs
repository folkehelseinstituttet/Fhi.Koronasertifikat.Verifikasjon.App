using System;
using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;
using FHICORC.ViewModels.Menu;
using FHICORC.ViewModels.QrScannerViewModels;
using FHICORC.Views.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);
        }

        void Switch_Toggled_Sound(object sender, ToggledEventArgs e)
        {
            ((SettingsPageViewModel)BindingContext).SoundSettingChanged(e.Value);
        }

        void Switch_Toggled_Vibration(object sender, ToggledEventArgs e)
        {
            ((SettingsPageViewModel)BindingContext).VibrationSettingChanged(e.Value);
        }

        void TapGestureRecognizer_Tapped_Bokmal(object sender, EventArgs e)
        {
            if (!RadioButtonBokmal.IsChecked)
            {
                RadioButtonBokmal.IsChecked = true;
            }
        }

        void TapGestureRecognizer_Tapped_Nynorsk(object sender, EventArgs e)
        {
            if (!RadioButtonNynorsk.IsChecked)
            {
                RadioButtonNynorsk.IsChecked = true;
            }
        }

        void TapGestureRecognizer_Tapped_English(object sender, EventArgs e)
        {
            if (!RadioButtonEnglish.IsChecked)
            {
                RadioButtonEnglish.IsChecked = true;
            }
        }
    }
}