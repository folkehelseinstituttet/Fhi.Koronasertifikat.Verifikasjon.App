using System;
using CoreGraphics;
using FHICORC.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace FHICORC.iOS
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && this.Control != null)
            {
                this.Control.LeftView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.RightView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                this.Control.LeftViewMode = UITextFieldViewMode.Always;
                this.Control.RightViewMode = UITextFieldViewMode.Always;

                this.Control.BorderStyle = UITextBorderStyle.None;
                this.Element.HeightRequest = 50;

            }        
        }
    }
}
