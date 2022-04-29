using System;
using Foundation;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Data;
using FHICORC.iOS.Services;
using FHICORC.Services.Interfaces;
using UIKit;
using Xamarin.Forms;

namespace FHICORC.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        UIVisualEffectView _blurWindow = null;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "Expander_Experimental" });

            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            AiForms.Dialogs.Dialogs.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            RegisterIosServices();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void RegisterIosServices()
        {
            IoCContainer.RegisterSingleton<IConfigurationProvider, ConfigurationProvider>();
            IoCContainer.RegisterSingleton<IStatusBarService, StatusBarService>();
            IoCContainer.RegisterSingleton<IPlatformBrowserService, PlatformBrowserService>(); 
            IoCContainer.RegisterInterface<IScannerFactory, IosScannerFactory>();
            IoCContainer.RegisterInterface<IDeepLinkingService, IosDeepLinkingService>();
            IoCContainer.RegisterInterface<ISqlConnection, SqlConnection>();
        }
        
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return OpenUrl(url);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return OpenUrl(url);
        }

        bool OpenUrl(NSUrl url)
        {
            try
            {
                // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
                Uri uri_netfx = new Uri(url.AbsoluteString);
            }
            catch (Exception)
            {

            }

            return true;
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);

            _blurWindow?.RemoveFromSuperview();
            _blurWindow?.Dispose();
            _blurWindow = null;

            MessagingCenter.Send<object>(this, MessagingCenterKeys.BACK_FROM_BACKGROUND);
        }

        public override void OnResignActivation(UIApplication uiApplication)
        {
            base.OnResignActivation(uiApplication);

            using (var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Prominent))
            {
                _blurWindow = new UIVisualEffectView(blurEffect)
                {
                    Frame = UIApplication.SharedApplication.KeyWindow.RootViewController.View.Bounds
                };
            UIApplication.SharedApplication.KeyWindow.AddSubview(_blurWindow);
            }
        }

    }
}
