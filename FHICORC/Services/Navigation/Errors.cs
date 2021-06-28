using System;
using FHICORC.Enums;
using FHICORC.Models;

namespace FHICORC.Services.Navigation
{
    public static class Errors
    {
        public static readonly ErrorPageModel UnknownError = new ErrorPageModel
        {
            Title = "ERROR_GENERIC_TITLE".Translate(),
            Message = "ERROR_GENERIC_TEXT".Translate(),
            Image = FHICORCImage.ErrorUnknown.Image(),
            ButtonTitle = "ERROR_OK_BUTTON".Translate(),
            HasSecondButton = false
        };
        public static readonly ErrorPageModel NoInternetError = new ErrorPageModel
        {
            Title = "ERROR_NO_INTERNET_TITLE".Translate(),
            Message = "ERROR_NO_INTERNET_TEXT".Translate(),
            Image = FHICORCImage.ErrorUnknown.Image(),
            ButtonTitle = "ERROR_TRY_AGAIN_BUTTON".Translate(),
            SecondButtonTitle = "CLOSE_BUTTON".Translate(),
            HasSecondButton = true,
            Type = ErrorPageType.NoInternet
        };
        public static readonly ErrorPageModel ForceUpdateRequiredError = new ErrorPageModel
        {
            Title = "ERROR_TITLE_FORCE_UPDATE_REQUIRED".Translate(),
            Message = "ERROR_SUBTITLE_FORCE_UPDATE_REQUIRED".Translate(),
            Image = FHICORCImage.ErrorMaintenance.Image(),
            ButtonTitle = "ERROR_BUTTON_FORCE_UPDATE_REQUIRED".Translate(),
            HasSecondButton = false,
            Type = ErrorPageType.ForceUpdate
        };


    }
}
