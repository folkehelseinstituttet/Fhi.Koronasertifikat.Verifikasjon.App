using System.Threading.Tasks;
using FHICORC.iOS.Services;
using FHICORC.Services.Interfaces;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformBrowserService))]
namespace FHICORC.iOS.Services
{
    public class PlatformBrowserService : IPlatformBrowserService
    {
        public Task CloseBrowser()
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            UIViewController vc = window.RootViewController;

            return vc.DismissViewControllerAsync(true);
        }
    }
}