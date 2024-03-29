﻿using System;
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
            SetAccessibilityText();
        }

        private void SetAccessibilityText()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                AutomationProperties.SetName(DateOfBirthLabel, ((ScanEuVaccineResultViewModel)BindingContext).DateOfBirthAccessibilityText);
                AutomationProperties.SetName(VaccineVaccinationDate, ((ScanEuVaccineResultViewModel)BindingContext).VaccineVaccinationDateValueAccessibilityText);
            }
            else
            {
                AutomationProperties.SetName(DateOfBirthLabel, "");
                AutomationProperties.SetName(VaccineVaccinationDate, "");
            }
        }
    }
}