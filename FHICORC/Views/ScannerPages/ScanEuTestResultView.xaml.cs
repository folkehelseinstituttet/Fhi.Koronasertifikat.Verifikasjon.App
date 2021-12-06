using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.ScannerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanEuTestResultView : ContentPage, IScanResultView
    {
        public ScanEuTestResultView()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ScanEuTestResultViewModel>();
        }

        protected override void OnDisappearing()
        {
            ((ScanEuTestResultViewModel)BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            ((ScanEuTestResultViewModel)BindingContext).Timer.Enabled = true;
            base.OnAppearing();
            SetAccessibilityText();
        }

        private void SetAccessibilityText()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                AutomationProperties.SetName(DateOfBirthLabel, ((ScanEuTestResultViewModel)BindingContext).DateOfBirthAccessibilityText);
            }
            else
            {
                AutomationProperties.SetName(DateOfBirthLabel, "");
            }
        }
    }
}