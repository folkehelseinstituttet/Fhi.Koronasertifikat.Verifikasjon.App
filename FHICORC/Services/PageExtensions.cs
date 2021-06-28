using System;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FHICORC.Services
{
    static class PageNavigationExtensions
    {
        public static bool IsModal(this Xamarin.Forms.Page page)
        {
            return (NavigationService.ModalPages.Any() && NavigationService.ModalPages.Peek() == page);
        }

        public static async Task Initialize(this Xamarin.Forms.Page page, object data)
        {
            if (data != null)
            {
                await ((BaseViewModel)page.BindingContext).InitializeAsync(data);
            }
        }

        public static void SetNavigationStyleFullscreen(this Xamarin.Forms.Page page)
        {
            page.On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FullScreen);
        }

        public static void SetNavigationStyleCardView(this Xamarin.Forms.Page page)
        {
            page.On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);
        }
    }
}
