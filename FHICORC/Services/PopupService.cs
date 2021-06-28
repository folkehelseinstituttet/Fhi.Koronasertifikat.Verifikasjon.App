using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Services.Interfaces;
using FHICORC.Views.Elements;

namespace FHICORC.Services
{
    public class PopupService : IPopupService
    {
        public Task ShowScanSuccessPopup(ITokenPayload payload)
        {
            return ScanSuccessResultPopup.ShowResult(payload);
        }
    }
}