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
    public partial class ScanEuRecoveryResultView : ContentPage, IScanResultView
    {
        public ScanEuRecoveryResultView()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ScanEuRecoveryResultViewModel>();
        }

        protected override void OnDisappearing()
        {
            ((ScanEuRecoveryResultViewModel)BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            ((ScanEuRecoveryResultViewModel)BindingContext).Timer.Enabled = true;
            base.OnAppearing();
            SetAccessibilityText();
        }

        private void SetAccessibilityText()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                AutomationProperties.SetName(DateOfBirthLabel, ((ScanEuRecoveryResultViewModel)BindingContext).DateOfBirthAccessibilityText);
            }
            else
            {
                AutomationProperties.SetName(DateOfBirthLabel, "");
            }
        }
    }
}