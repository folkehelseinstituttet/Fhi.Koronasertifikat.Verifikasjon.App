using System;
using System.Collections.Generic;
using FHICORC.Configuration;
using FHICORC.ViewModels;
using Xamarin.Forms;

namespace FHICORC.Views
{
    public partial class AcceptTermsPage : ContentPage
    {
        public AcceptTermsPage()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<AcceptTermsViewModel>();
        }
    }
}
