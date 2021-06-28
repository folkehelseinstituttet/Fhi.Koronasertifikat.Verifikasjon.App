using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Base
{
    public class ContentSheetPageNoBackButtonOnIOSViewModel : BaseViewModel
    {
        bool _backButtonUsed;

        public override ICommand BackCommand => new Command(async () => await ExecuteOnceAsync(BackButtonClicked));

        private async Task BackButtonClicked()
        {
            _backButtonUsed = true;
            await _navigationService.PopPage();
        }

        public void OnModalDismissed()
        {
            if (!_backButtonUsed)
            {
                _navigationService.OnDidDismissSheetPageOnIOS();
            }
        }
    }
}
