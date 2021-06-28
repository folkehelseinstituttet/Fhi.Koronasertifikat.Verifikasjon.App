using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FHICORC.Services.Interfaces;
using Foundation;
using UIKit;

namespace FHICORC.iOS.Services
{
    public class IosDeepLinkingService : IDeepLinkingService
    {
        public async Task GoToAppSettings()
        {
            try
            {
                await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(UIApplication.OpenSettingsUrlString),
                    new UIApplicationOpenUrlOptions());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error attempting to deep-link to application settings: {ex}");
            }
        }
    }
}
