using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace FHICORC.Utils
{
    public enum FHICORCColor
    {
        LinkColor,
        LightTextColor,
        ContentTextColor,
        TitleTextColor,
        BaseTextColor,
        DefaultBackgroundColor,
        NavigationHeaderBackgroundColor,
        FHIPrimaryBlue,
        ButtonBlue,
        ButtonDisable,

        LandingPageColorStart,
        SuccessBorderColor,
        InvalidBorderColor,
        ExpiredBorderColor
    }

    public static class FHICORCColorExtensions
    {
        public static Color Color(this FHICORCColor color)
        {
            string colourString = Enum.GetName(typeof(FHICORCColor), color);
            Color? colour = Application.Current.Resources[colourString] as Color?;
            return colour ?? Xamarin.Forms.Color.White;
        }
    }
}
