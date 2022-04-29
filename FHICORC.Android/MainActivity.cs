using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Plugin.CurrentActivity;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Droid.Services;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace FHICORC.Droid
{
    [Activity(
        Label = "Kontroll av koronasertifikat",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round",
        Theme = "@style/MainTheme",
        NoHistory = false,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ICloseButtonService
    {
        INavigationService _navigationService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "Expander_Experimental" });

#if !DEBUG && !TEST
            Window.AddFlags(WindowManagerFlags.Secure);
#endif

            CrossCurrentActivity.Current.Activity = this;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            }

            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            AndroidEnvironment.UnhandledExceptionRaiser += OnUnhandledAndroidException;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AiForms.Dialogs.Dialogs.Init(this);
            RegisterAndroidServices();

            LoadApplication(new App());
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            var settingsService = IoCContainer.Resolve<ISettingsService>();
            _navigationService = IoCContainer.Resolve<INavigationService>();
        }

        private void RegisterAndroidServices()
        {
            IoCContainer.RegisterSingleton<IConfigurationProvider, ConfigurationProvider>();
            IoCContainer.RegisterSingleton<IStatusBarService, StatusBarService>();
            IoCContainer.RegisterInstance<ICloseButtonService>(this);
            IoCContainer.RegisterInterface<IScannerFactory, AndroidScannerFactory>();
            IoCContainer.RegisterInterface<IDeepLinkingService, AndroidDeepLinkingService>();
            IoCContainer.RegisterInterface<ISqlConnection, SqlConnection>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            AndroidEnvironment.UnhandledExceptionRaiser -= OnUnhandledAndroidException;
            base.OnDestroy();
        }

        private void OnUnhandledAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            if (e?.Exception != null)
            {
                string message = $"{nameof(MainActivity)}.{nameof(OnUnhandledAndroidException)}: "
                    + (!e.Handled
                    ? "Native unhandled crash"
                    : "Native unhandled exception - not crashing");
            }
        }

        public override void OnBackPressed()
        {
            if (_navigationService != null)
            {
                var currentPage = _navigationService.FindCurrentPage();
                if (currentPage != null && currentPage.BindingContext is BaseViewModel vm)
                {
                    //The BackCommand on the current active VM will be executed instead of the native back command.
                    bool didPop = Rg.Plugins.Popup.Popup.SendBackPressed(() => vm.BackCommand.Execute(null));

                }
            }
        }

        public void ClickCloseButton()
        {
            base.OnBackPressed();
        }
    }
}