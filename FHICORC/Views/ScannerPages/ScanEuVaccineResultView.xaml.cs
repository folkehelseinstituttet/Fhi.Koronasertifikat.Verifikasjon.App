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
    public partial class ScanEuVaccineResultView : ContentPage, IScanResultView
    {
        public ScanEuVaccineResultView()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<ScanEuVaccineResultViewModel>();
        }

        protected override void OnDisappearing()
        {
            ((ScanEuVaccineResultViewModel)BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            ((ScanEuVaccineResultViewModel)BindingContext).Timer.Enabled = true;
            base.OnAppearing();
        }
    }
}