using System;
using FHICORC.iOS;
using FHICORC.Services.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof (StatusBarService))]
namespace FHICORC.iOS
{
    public class StatusBarService : IStatusBarService
    {
        public StatusBarService()
        {
        }

        public void SetStatusBarColor(Color backgroundColor, Color textColor)
        {
            if (textColor == Color.White)
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            else if (textColor == Color.Black)
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.DarkContent;
        }

    }
}
