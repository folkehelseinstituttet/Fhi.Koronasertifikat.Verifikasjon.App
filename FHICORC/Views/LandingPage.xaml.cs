using System;
using FHICORC.Configuration;
using FHICORC.Controls;
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
        private Label _label;
        private Image _fhiImage;
        private ImageButton _helpImageButton;
        private double _fontSize;
        private ImageButtonView _imageButtonView;
        private SingleLineButton _singleLineButton;

        public LandingPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<LandingViewModel>();
            _label = this.FindByName<Label>("TitleLbl");
            _fhiImage = this.FindByName<Image>("FhiImg");
            _helpImageButton = this.FindByName<ImageButton>("HelpBtn");
            _imageButtonView = this.FindByName<ImageButtonView>("ChangeLanguageImageButton");
            _singleLineButton = this.FindByName<SingleLineButton>("ChangeLanguageSingleLineButton");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IoCContainer.Resolve<INavigationService>().SetStatusBar(FHICORCColor.LandingPageColorStart.Color(), Color.White);
            _fontSize = _label.FontSize;
            UpdateIconSizes();
            if (Device.RuntimePlatform == Device.iOS)
            {
                IOSAccessibilityButton();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                _singleLineButton.IsVisible = true;
                _imageButtonView.IsVisible = false;
            }
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
        private void UpdateIconSizes()
        {

            const int threshold = 22;
            double delta = _fontSize - threshold;
            double scaleValue = 1.0;
            Thickness thickness = _fhiImage.Margin;
            const double defaultThicknessLeft = 20.0;
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
            thickness.Left = defaultThicknessLeft * scaleValue;
            _fhiImage.Margin = thickness;
            _fhiImage.Scale = scaleValue;
            _helpImageButton.Scale = scaleValue;
        }
        private void IOSAccessibilityButton()
        {
            Thickness thickness = _imageButtonView.Margin;
            if (_fontSize > 29.00)
            {
                _imageButtonView.IsVisible = true;
                _singleLineButton.IsVisible = false;
                thickness.Top = -_singleLineButton.Height; 
            }
            else
            {
                _imageButtonView.IsVisible = false;
                _singleLineButton.IsVisible = true;
                thickness.Top = 0;
            }
            _imageButtonView.Margin = thickness;
        }
    }
}