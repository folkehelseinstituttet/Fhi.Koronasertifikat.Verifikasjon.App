using System;
using FHICORC.Configuration;
using FHICORC.Controls;
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
        private Label _labelSettings;
        private Switch _switchToggledSound;
        private Switch _switchToggledVibration;
        private SingleLineButton _fetchSingleLineButton;
        private double _fontSize;

        public SettingsPage(SettingsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _labelSettings = this.FindByName<Label>("LabelSettings");
            _switchToggledSound = this.FindByName<Switch>("SwitchToggledSound");
            _switchToggledVibration = this.FindByName<Switch>("SwitchToggledVibration");
            _fetchSingleLineButton = this.FindByName<SingleLineButton>("FetchSingleLineButton");
            _fontSize = _labelSettings.FontSize;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.NavigationHeaderBackgroundColor.Color(), Color.Black);
            UpdateIconsSizesAccessibility();

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
        private void UpdateIconsSizesAccessibility()
        {
            int threshold = 22;
            double delta = _fontSize - threshold;
            double scaleValue = 1.0;

            if (Device.RuntimePlatform == Device.iOS && _fontSize > threshold)
            {
                scaleValue = 1.0 + 0.01 * delta;

            }
            else if (Device.RuntimePlatform == Device.Android && _fontSize > threshold)
            {
                scaleValue = 1.0 + 0.015 * delta;
            }
            
            if (scaleValue >= 1.4)
            {
                scaleValue = 1.4;
            }
            else if (scaleValue < 1.0)
            {
                scaleValue = 1.0;
            }
            double scaleValueButton = scaleValue;
            if (scaleValueButton >= 1.1)
            {
                scaleValueButton = 1.1;
            }
            _switchToggledSound.Scale = scaleValue;
            _switchToggledVibration.Scale = scaleValue;
            _fetchSingleLineButton.Scale = scaleValueButton;

        }
    }
}