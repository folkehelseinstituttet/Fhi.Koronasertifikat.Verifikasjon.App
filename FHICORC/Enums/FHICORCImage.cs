using System;
using Xamarin.Forms;

namespace FHICORC.Enums
{
    public enum FHICORCImage
    {
        ErrorMaintenance,
        ErrorQueue,
        ErrorUnknown,
        ErrorLock
    }

    public static class FHICORCSImageExtensions
    {
        public static string Image(this FHICORCImage img)
        {
            string imageString = Enum.GetName(typeof(FHICORCImage), img);
            string image = Application.Current.Resources[imageString] as string;
            return image; 
        }
    }

}