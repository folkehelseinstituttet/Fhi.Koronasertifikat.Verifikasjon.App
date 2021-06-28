using FHICORC.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FontSizeLabelEffect = FHICORC.iOS.FontSizeLabelEffect;

[assembly: ResolutionGroupName("FHICORC")]
[assembly: ExportEffect(typeof(FontSizeLabelEffect), nameof(FontSizeLabelEffect))]
namespace FHICORC.iOS
{
    public class FontSizeLabelEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var min = (int)FontSizeLabelEffectParams.GetMinFontSize(this.Element);
            var max = (int)FontSizeLabelEffectParams.GetMaxFontSize(this.Element);

            if (max <= min)
                return;
            if (this.Control is UILabel label)
            {
                label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
                label.LineBreakMode = UILineBreakMode.Clip;
               
                var currentFontSize = (int)label.Font.PointSize;
                if (currentFontSize > max)
                {
                    label.AdjustsFontSizeToFitWidth = true;
                    label.MinimumFontSize = (float)min;
                    label.Font = label.Font.WithSize((float)max);
                }
            }
            if (this.Control is UIButton button)
            {
                button.LineBreakMode = UILineBreakMode.Clip;
                
                var currentFontSize = (int)button.Font.PointSize;
                if (currentFontSize > max)
                {
                    button.TitleLabel.AdjustsFontSizeToFitWidth = true;
                    button.TitleLabel.MinimumFontSize = (float)min;
                    button.Font = button.Font.WithSize((float)max);
                }
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
