using System;
using Android.Content;
using Android.Widget;
using FHICORC.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Switch), typeof(CustomSwitchRenderer))]
namespace FHICORC.Droid.CustomRenderers
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        public CustomSwitchRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (Control.Checked)
                {
                    Control.SetTrackResource(Resource.Drawable.track);
                    Control.SetThumbResource(Resource.Drawable.thumb);
                }
                else
                {
                    Control.SetTrackResource(Resource.Drawable.track);
                    Control.SetThumbResource(Resource.Drawable.thumb);
                }
            }
            Control.CheckedChange += OnCheckedChange;
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (Control.Checked)
            {
                Control.SetTrackResource(Resource.Drawable.track);
                Control.SetThumbResource(Resource.Drawable.thumb);
            } else
            {
                Control.SetTrackResource(Resource.Drawable.track);
                Control.SetThumbResource(Resource.Drawable.thumb);
            }
            Element.IsToggled = Control.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            Control.CheckedChange -= OnCheckedChange;
            base.Dispose(disposing);
        }
    }
}
