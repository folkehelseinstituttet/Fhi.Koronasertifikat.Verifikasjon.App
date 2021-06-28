using System;
using Xamarin.Forms;
using FHICORC.Enums;

namespace FHICORC.Models
{
    public class ErrorPageModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public string ButtonTitle { get; set; }
        public string SecondButtonTitle { get; set; }
        public bool HasSecondButton { get; set; }
        public ErrorPageType Type { get; set; } = ErrorPageType.Default;
    }
}
