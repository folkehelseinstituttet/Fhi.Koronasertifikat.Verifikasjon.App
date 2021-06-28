using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using FHICORC.Services;
using FHICORC.Utils;
using FHICORC.ViewModels.Base;
using Xamarin.Forms;
using FHICORC.Core.Services.Model.NO;
using FHICORC.Services.Interfaces;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class ScanSuccessResultPopupViewModel : BaseScanViewModel
    {
        public string ValidText => "VALID_RESULT".Translate();
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string SuccessBannerText => string.Concat(Enumerable.Repeat($"{ValidText}      ", 10));

        public ICommand ClosePopupCommand => new Command(CloseResultPopup);

        public ScanSuccessResultPopupViewModel(ITimer timer) : base(timer)
        {
            Timer.OnStop = CloseResultPopup;
        }

        private void CloseResultPopup()
        {
            PopupNavigation.Instance.PopAllAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is NO1Payload no1)
            {
                FullName = no1.DGCPayloadData.DGC.PersonName.FullName;
                DateOfBirth = no1.DGCPayloadData.DGC.DateOfBirth;

                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(DateOfBirth));
            }

            OnPropertyChanged(nameof(ValidText));
            OnPropertyChanged(nameof(SuccessBannerText));

            return base.InitializeAsync(navigationData);
        }
    }
}