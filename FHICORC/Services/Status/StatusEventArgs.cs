using System;

namespace FHICORC.Services.Status
{
    /// <summary>
    /// Custom event args for use by the scanner
    /// </summary>
    public class StatusEventArgs : EventArgs
    {
        private string barcodeData;

        public StatusEventArgs(string dataIn)
        {
            barcodeData = dataIn;
        }

        public string Data { get { return barcodeData; } }

    }

}
