using System;
using FHICORC.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Switch), typeof(CustomSwitchRenderer))]
namespace FHICORC.iOS.CustomRenderers
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            
            base.OnElementChanged(e);

            if (Control != null)
            {
                UpdateUISwitchColor();
                Element.Toggled += ElementToggled;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (Control != null)
            {
                Element.Toggled -= ElementToggled;
            }
        }

        private void ElementToggled(object sender, ToggledEventArgs e)
        {
            UpdateUISwitchColor();
        }

        private void UpdateUISwitchColor()
        {
            var temp = Element as Switch;

            if (temp.IsToggled)
            {
                Control.ThumbTintColor = Color.FromHex("#F3F9FB").ToUIColor();
                Control.OnTintColor = Color.FromHex("#32345C").ToUIColor();
            }
            else
            {
                Control.ThumbTintColor = Color.FromHex("#FFFFFF").ToUIColor();
            }
        }
    }
}
