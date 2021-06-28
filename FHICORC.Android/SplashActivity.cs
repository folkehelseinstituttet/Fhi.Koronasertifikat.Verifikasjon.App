using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
namespace FHICORC.Droid
{
    [Activity(
        Label = "Kontroll av koronasertifikat",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round",
        Theme = "@style/FHICORC.Splash",
        MainLauncher = true,
        NoHistory = true,
        LaunchMode = LaunchMode.SingleInstance
    )]
    public class SplashActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public override void OnBackPressed() { }
    }
}