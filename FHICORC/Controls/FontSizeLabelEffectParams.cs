using Xamarin.Forms;

namespace FHICORC.Controls
{
    public static class FontSizeLabelEffectParams
    {
        #region Public Fields

        public static BindableProperty MaxFontSizeProperty = BindableProperty.CreateAttached("MaxFontSize", typeof(double), typeof(FontSizeLabelEffectParams), (double)32, BindingMode.Default);

        public static BindableProperty MinFontSizeProperty = BindableProperty.CreateAttached("MinFontSize", typeof(double), typeof(FontSizeLabelEffectParams), (double)12, BindingMode.Default);

        #endregion Public Fields

        #region Public Methods
        
        public static double GetMaxFontSize(BindableObject bindable)
        {
            double maxFontSize = (double)bindable.GetValue(MaxFontSizeProperty);
            return maxFontSize;
        }

        public static double GetMinFontSize(BindableObject bindable)
        {
            double minFontSize = (double)bindable.GetValue(MinFontSizeProperty);
            return minFontSize;
        }


        public static void SetMaxFontSize(BindableObject bindable, double value)
        {
            bindable.SetValue(MaxFontSizeProperty, value);
        }

        public static void SetMinFontSize(BindableObject bindable, double value)
        {
            bindable.SetValue(MinFontSizeProperty, value);
        }

        #endregion Public Methods
    }
}