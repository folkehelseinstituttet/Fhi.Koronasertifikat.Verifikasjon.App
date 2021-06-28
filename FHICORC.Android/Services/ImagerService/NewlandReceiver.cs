using System;
using Android.Content;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Status;

namespace FHICORC.Droid.Services.ImagerService
{
    [BroadcastReceiver]
    public class NewlandReceiver : BroadcastReceiver, IImagerReceiver
    {
        public static string INTENT_RESULT_ACTION = "nlscan.action.SCANNER_RESULT";
        public event EventHandler<StatusEventArgs> OnBarcodeScanned;

        public override void OnReceive(Context context, Intent intent)
        {
            // check the intent action is for us
            if (intent.Action == INTENT_RESULT_ACTION)
            {
                OnReceiveBarcodeIntentResult(intent);
            }
        }

        private void OnReceiveBarcodeIntentResult(Intent intent)
        {
            if (intent == null)
            {
                return;
            }

            string barcode = intent.GetStringExtra("SCAN_BARCODE1");
            int barcodeType = intent.GetIntExtra("SCAN_BARCODE_TYPE", -1);

            if (barcode == null)
            {
                return;
            }

            if (OnBarcodeScanned != null && barcodeType == 258)
            {
                OnBarcodeScanned(this, new StatusEventArgs(barcode));
            }
        }
    }
}