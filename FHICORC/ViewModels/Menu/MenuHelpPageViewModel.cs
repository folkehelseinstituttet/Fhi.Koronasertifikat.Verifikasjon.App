using System.Windows.Input;
using FHICORC.Services;
using FHICORC.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Menu
{
    public class MenuHelpPageViewModel : BaseViewModel
    {
        public string HelpUrl => "HELP_URL".Translate();
        public string PrivacyUrl => "PRIVACY_URL".Translate();
        public string AccessibilityUrl => "ACCESSIBILITY_URL".Translate();

        public ICommand OpenHelpLinkCommand => new Command(async () => await Launcher.OpenAsync(HelpUrl));
        public ICommand OpenPrivacyLinkCommand => new Command(async () => await Launcher.OpenAsync(PrivacyUrl));
        public ICommand OpenAccessibilityLinkCommand => new Command(async () => await Launcher.OpenAsync(AccessibilityUrl));

    }
}