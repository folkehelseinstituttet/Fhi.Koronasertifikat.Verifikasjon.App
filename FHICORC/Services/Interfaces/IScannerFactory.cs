
#nullable enable
namespace FHICORC.Services.Interfaces
{
    public interface IScannerFactory
    {
        IImagerScanner? GetAvailableScanner();
        IScannerConfig? GetScannerConfig();
    }
}