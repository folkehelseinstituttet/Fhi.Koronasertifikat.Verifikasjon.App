using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.ViewModels.QrScannerViewModels;
using Xamarin.Forms;

namespace FHICORC.Views.Elements
{
    public partial class ScanSuccessResultPopup : PopupPage
    {
        public static async Task ShowResult(ITokenPayload payload)
        {
            await PopupNavigation.Instance.PushAsync(new ScanSuccessResultPopup(payload));
        }

        public ScanSuccessResultPopup(ITokenPayload payload)
        {
            InitializeComponent();
            AndroidTalkbackAccessibilityWorkaround = true;

            ScanSuccessResultPopupViewModel viewModel = IoCContainer.Resolve<ScanSuccessResultPopupViewModel>();
            viewModel.InitializeAsync(payload);
            BindingContext = viewModel;
            
            ProgressBar.ProgressTo(1.0,
                Convert.ToUInt32(IoCContainer.Resolve<ISettingsService>().ScannerShownDurationMs), Easing.Linear);
        }

        protected override void OnDisappearing()
        {
            ((ScanSuccessResultPopupViewModel) BindingContext).Timer.Enabled = false;
            base.OnDisappearing();
        }

        void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            ((ScanSuccessResultPopupViewModel)BindingContext).ClosePopupCommand.Execute(null);
        }
    }
}