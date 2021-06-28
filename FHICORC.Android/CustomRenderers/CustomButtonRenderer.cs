using Android.Content;
using Android.Views;
using FHICORC.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace FHICORC.Droid
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            Control?.SetPadding(0, 0, 0, 0);

            if (Control != null && e.NewElement != null)
            {
                Control.Touch += OnTouch;
            }
        }
        
        private void OnTouch(object sender, TouchEventArgs args)
        {
            var buttonController = (IButtonController)Element;
            if (buttonController == null)
                return;

            var x = (int)args.Event.GetX();
            var y = (int)args.Event.GetY();

            if (!TouchInsideControl(x, y))
            {
                buttonController.SendReleased();
            }
            else if (args.Event.Action == MotionEventActions.Down)
            {
                buttonController.SendPressed();
            }
            else if (args.Event.Action == MotionEventActions.Up)
            {
                buttonController.SendReleased();
                buttonController.SendClicked();
            }
            else
            {
                buttonController.SendPressed();
            }
        }

        private bool TouchInsideControl(int x, int y)
        {
            return x <= Control.Right && x >= Control.Left && y <= Control.Bottom && y >= Control.Top;
        }
    }
}