using System;
using System.Collections.Generic;
using FHICORC.Enums;
using FHICORC.ViewModels.Certificates;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;

namespace FHICORC.Views.ScannerPages
{
    public partial class RulesInfoModalView : ContentSheetPageNoBackButtonOnIOS
    {
        private RulesInfoModalViewModel _viewModel;

        public RulesInfoModalView(RulesFeedbackViewModel viewModel, EuPassportType euPassportType)
        {
            InitializeComponent();
            BindingContext = _viewModel = new RulesInfoModalViewModel(viewModel, euPassportType);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.UpdateView();
        }
    }
}