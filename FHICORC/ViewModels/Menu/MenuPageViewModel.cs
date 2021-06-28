using System.Windows.Input;
using FHICORC.ViewModels.Base;
using FHICORC.Views.Menu;
using Xamarin.Forms;
using Xamarin.Essentials;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;

namespace FHICORC.ViewModels.Menu
{
    public class MenuPageViewModel : BaseViewModel
    {           
        public virtual ICommand OpenSettingsPage => new Command(async () => await ExecuteOnceAsync(async () => await _navigationService.PushPage(new SettingsPage(IoCContainer.Resolve<SettingsPageViewModel>()))));
        
        public ICommand OpenSupportPage => new Command(async () => await ExecuteOnceAsync(async () => await _navigationService.PushPage(new HelpPage())));

#if APPSTORE
        public string VersionNumber  =>  string.Format("Version {0}", VersionTracking.CurrentVersion);
#else
        public string VersionNumber => string.Format(
                "Version {0} ({1}) - {2} - {3}",
                VersionTracking.CurrentVersion,
                VersionTracking.CurrentBuild,
                IoCContainer.Resolve<ISettingsService>().Environment,
                IoCContainer.Resolve<ISettingsService>().ApiVersion
            );
#endif
        
        private bool _isLoggedIn = true;

        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }
        
    }
}