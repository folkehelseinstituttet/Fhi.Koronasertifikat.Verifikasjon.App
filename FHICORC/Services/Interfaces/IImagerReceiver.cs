using FHICORC.Services.Status;
using System;

namespace FHICORC.Services.Interfaces
{
    public interface IImagerReceiver
    {
        event EventHandler<StatusEventArgs> OnBarcodeScanned;
    }
}