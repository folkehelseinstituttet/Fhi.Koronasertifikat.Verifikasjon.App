using FHICORC.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class SplashViewModel : BaseViewModel
    {
        private string _loadingText;

        public string LoadingText
        {
            get
            {
                return _loadingText;
            }
            set
            {
                _loadingText = value;
                RaisePropertyChanged(() => LoadingText);
            }
        }

        public SplashViewModel()
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            var message = navigationData as string;
            SetLoadingText(message);
            return base.InitializeAsync(navigationData);
        }
        public void SetLoadingText(string message)
        {
            LoadingText = message;
        }
    }
}
