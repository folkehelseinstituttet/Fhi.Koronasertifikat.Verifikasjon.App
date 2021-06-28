#nullable enable
using FHICORC.Services.Interfaces;

namespace FHICORC.iOS.Services
{
    public class IosScannerFactory: IScannerFactory
    {
        public IImagerScanner? GetAvailableScanner()
        {
            return null; // currently no support for scanner in ios devices
        }

        public IScannerConfig? GetScannerConfig()
        {
            return null;
        }
    }
}