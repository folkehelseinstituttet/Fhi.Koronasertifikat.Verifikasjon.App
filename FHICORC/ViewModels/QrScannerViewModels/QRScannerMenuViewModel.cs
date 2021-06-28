using System;
using System.Windows.Input;
using FHICORC.Configuration;
using FHICORC.ViewModels.Menu;
using FHICORC.Views.Menu;
using Xamarin.Forms;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class QRScannerMenuViewModel: MenuPageViewModel
    {
        public QRScannerMenuViewModel()
        {
            IsLoggedIn = false;
        }

        public override ICommand OpenSettingsPage => new Command(
            async () => await ExecuteOnceAsync(
                async () => await _navigationService.PushPage(
                    new SettingsPage(IoCContainer.Resolve<QRScannerSettingsViewModel>()))));

    }
}
