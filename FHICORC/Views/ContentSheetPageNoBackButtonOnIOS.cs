using System;
using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FHICORC.Views
{
    /// <summary>
    /// Used for displaying an iOS page with a card view / Drawer from the buttom.
    /// Also see NavigationService.PushPage
    /// NOTE: This only works as long as there is no back button on iOS. Otherwise the NavigationService will be called twice when popping.
    /// 
    /// It does nothing for Android.
    /// </summary>
    public class ContentSheetPageNoBackButtonOnIOS : ContentPage
    {
        public ContentSheetPageNoBackButtonOnIOS() : base()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as ContentSheetPageNoBackButtonOnIOSViewModel)?.OnModalDismissed();
        }
    }
}
