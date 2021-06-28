using Android.Util;
using Android.Widget;
using Plugin.CurrentActivity;
using FHICORC.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Android.Widget.Button;

[assembly: ResolutionGroupName("FHICORC")]
[assembly: ExportEffect(typeof(FHICORC.Droid.FontSizeLabelEffect), nameof(FHICORC.Controls.FontSizeLabelEffect))]
namespace FHICORC.Droid
{
    public class FontSizeLabelEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var min = (int)FontSizeLabelEffectParams.GetMinFontSize(this.Element);
            var max = (int)FontSizeLabelEffectParams.GetMaxFontSize(this.Element);

            var metrics = CrossCurrentActivity.Current.Activity.ApplicationContext.Resources.DisplayMetrics;

            if (max <= min)
                return;
            if (this.Control is TextView textView)
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    // SetAutoSizeTextTypeUniformWithConfiguration() is available from API level 26.
                    // https://developer.android.com/guide/topics/ui/look-and-feel/autosizing-textview
                    // On older Android devices, attempting to call this function and it throws an exception.

                    var systemFontScaleValue = Android.Content.Res.Resources.System.Configuration.FontScale;

                    //The sp value on Android is already a scalable value
                    var defaultSpValue = PxToSp(textView.TextSize, metrics);


                    if (systemFontScaleValue >= 1.3f)
                    {
                        //Apply effect when FontScale valeu larger than a certain amount
                        textView.SetAutoSizeTextTypeUniformWithConfiguration(5, defaultSpValue, 1,
                            (int)ComplexUnitType.Sp);
                    }
                }
            }
        }

        protected override void OnDetached()
        {
        }

        public int PxToSp(float px, DisplayMetrics metrics)
        {
            return (int)(px / metrics.ScaledDensity);
        }

    }
}