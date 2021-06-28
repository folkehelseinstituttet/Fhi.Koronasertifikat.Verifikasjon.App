using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Services.Interfaces
{
    public interface IPopupService
    {
        Task ShowScanSuccessPopup(ITokenPayload payload);
    }
}