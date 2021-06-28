using Android.OS;
using FHICORC.Droid;
using FHICORC.Services.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using FHICORC.Utils;

[assembly: Dependency(typeof(StatusBarService))]
namespace FHICORC.Droid
{
    public class StatusBarService : IStatusBarService
    {
        public StatusBarService()
        {
        }

        public void SetStatusBarColor(Color backgroundColor, Color textColor)
        {
            if (CrossCurrentActivity.Current?.Activity?.Window != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    if (textColor == Color.White)
                    {
                        CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility
                            = (StatusBarVisibility)StatusBarVisibility.Visible;

                    }
                    else
                    {
                        CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility
                            = (StatusBarVisibility)SystemUiFlags.LightStatusBar;

                    }

                    CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(backgroundColor.ToAndroid());
                }
            }
        }
    }
}