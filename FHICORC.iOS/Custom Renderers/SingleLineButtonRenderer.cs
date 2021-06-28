using System;
using CoreGraphics;
using FHICORC.Controls;
using FHICORC.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SingleLineButton), typeof(SingleLineButtonRenderer))]
namespace FHICORC.iOS.CustomRenderers
{
    public class SingleLineButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (Control.TitleLabel != null)
                Control.TitleLabel.AdjustsFontSizeToFitWidth = true;
            }
        }
    }
}
