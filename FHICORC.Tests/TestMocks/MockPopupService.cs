using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Services.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockPopupService : IPopupService
    {
        public Task ShowScanSuccessPopup(ITokenPayload payload)
        {
            return Task.FromResult(true);
        }
    }
}