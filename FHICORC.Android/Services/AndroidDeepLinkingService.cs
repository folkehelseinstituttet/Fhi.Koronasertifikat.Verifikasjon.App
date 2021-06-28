using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using FHICORC.Services.Interfaces;
using Plugin.CurrentActivity;

namespace FHICORC.Droid.Services
{
    public class AndroidDeepLinkingService : IDeepLinkingService
    {
        public Task GoToAppSettings()
        {
            try
            {
                var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
                intent.AddFlags(ActivityFlags.NewTask);
                string package_name = "no.fhi.KoronasertifikatKontrollapp";
                var uri = Android.Net.Uri.FromParts("package", package_name, null);
                intent.SetData(uri);
                CrossCurrentActivity.Current.AppContext.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error attempting to deep-link to application settings: {ex}");
            }

            return Task.CompletedTask;
        }
    }
}
