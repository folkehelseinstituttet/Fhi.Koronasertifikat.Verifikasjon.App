using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FHICORC.ViewModels;
using FHICORC.Configuration;

namespace FHICORC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialDataLoad : ContentPage
    {
        public InitialDataLoad()
        {
            InitializeComponent();
            BindingContext = IoCContainer.Resolve<InitialDataLoadViewModel>();
        }
    }
}

        
