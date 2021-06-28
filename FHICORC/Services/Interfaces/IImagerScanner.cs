using System;
using System.Collections.Generic;
using FHICORC.Services.Scanner;
using FHICORC.Services.Status;

namespace FHICORC.Services.Interfaces
{
    public interface IImagerScanner
    {
        void Enable();
        bool IsEnabled { get; }
        void Disable();
        void SetConfig(IScannerConfig config);
        void SetSelectedScanner(ScannerModel scannerModel);
        IImagerReceiver Receiver { get; }
    }
}