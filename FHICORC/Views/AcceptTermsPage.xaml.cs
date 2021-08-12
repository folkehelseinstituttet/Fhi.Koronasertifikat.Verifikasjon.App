using System;
using System.Collections.Generic;
using FHICORC.Configuration;
using FHICORC.ViewModels;
using Xamarin.Forms;

namespace FHICORC.Views
{
    public partial class AcceptTermsPage : ContentPage
    {
        private Label _acceptLabel;
        private Switch _toggleSwitch;
        private ImageButton _backImageButton;
        private double _fontSize;

        public AcceptTermsPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<AcceptTermsViewModel>();
            _acceptLabel = this.FindByName<Label>("AcceptLabel");
            _toggleSwitch = this.FindByName<Switch>("ToggleSwitch");
            _backImageButton = this.FindByName<ImageButton>("BackImageButton");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _fontSize = _acceptLabel.FontSize;
            UpdateSizeAccessibility();

        }
        private void UpdateSizeAccessibility()
        {
            const int threshold = 22;
            double delta = _fontSize - threshold;
            double scaleValue = 1.0;
            const double defaultThickness = 15.00;
            Thickness thickness = _backImageButton.Padding;
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
            thickness.Left = defaultThickness * scaleValue;
            thickness.Top = 2*defaultThickness - (defaultThickness * scaleValue);
            _toggleSwitch.Scale = scaleValue;
            _backImageButton.Scale = scaleValue;
            _backImageButton.Padding = thickness;
        }
    }
}
