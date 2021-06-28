using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Runtime;
using FHICORC.Services.Scanner;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Status;

namespace FHICORC.Droid.Services.ImagerService
{
    [BroadcastReceiver]
    public class DataWedgeReceiver : BroadcastReceiver, IImagerReceiver
    {
        // This intent string contains the source of the data as a string
        private static string SOURCE_TAG = "com.motorolasolutions.emdk.datawedge.source";
        // This intent string contains the barcode symbology as a string
        private static string LABEL_TYPE_TAG = "com.motorolasolutions.emdk.datawedge.label_type";
        // This intent string contains the captured data as a string
        // (in the case of MSR this data string contains a concatenation of the track data)
        private static string DATA_STRING_TAG = "com.motorolasolutions.emdk.datawedge.data_string";

        // Intent Action for our operation
        public static string INTENT_BARCODE_ACTION = "FHICORC_BARCODE.RECVR";
        public static string INTENT_RESULT_ACTION = "com.symbol.datawedge.api.RESULT_ACTION";
        public static string INTENT_SOFT_SCAN_ACTION = "com.symbol.datawedge.api.ACTION_SOFTSCANTRIGGER";
        public static string INTENT_CATEGORY = "android.intent.category.DEFAULT";

        public static string INTENT_RESULT_ENUMERATE_SCANNER = "com.symbol.datawedge.api.RESULT_ENUMERATE_SCANNERS";
        public event EventHandler<StatusEventArgs> OnBarcodeScanned;
        public event EventHandler<List<ScannerModel>> ScannersInfoReceived; // to be used when supporting multiple data wedges

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            // check the intent action is for us
            if (action.Equals(INTENT_BARCODE_ACTION))
            {
                OnReceiveBarcodeIntentResult(intent);
            }

            if (intent.HasExtra(INTENT_RESULT_ENUMERATE_SCANNER))
            {
                OnReceiveEnumerateScannerInfoIntentResult(intent);

            }
        }

        private void OnReceiveBarcodeIntentResult(Intent intent)
        {
            // define a string that will hold our output
            string Out = "";
            string sLabelType = "";
            // get the source of the data
            string source = intent.GetStringExtra(SOURCE_TAG);
            // save it to use later
            if (source == null)
            {
                source = "scanner";
            }
            // get the data from the intent
            string data = intent.GetStringExtra(DATA_STRING_TAG);
            // check if the data has come from the barcode scanner
            if (source != "scanner")
            {
                return;
            }
            // check if there is anything in the data
            if (data == null || data.Length <= 0)
            {
                return;
            }
            // we have some data, so let's get it's symbology
            sLabelType = intent.GetStringExtra(LABEL_TYPE_TAG);
            // check if the string is empty
            if (sLabelType != null && sLabelType.Length > 0)
            {
                // format of the label type string is LABEL-TYPE-SYMBOLOGY
                // so let's skip the LABEL-TYPE- portion to get just the symbology
                sLabelType = sLabelType.Substring(11);
            }
            else
            {
                // the string was empty so let's set it to "Unknown"
                sLabelType = "Unknown";
            }

            if (OnBarcodeScanned != null && sLabelType == "QRCODE")
            {
                OnBarcodeScanned(this, new StatusEventArgs(data.ToString()));
            }
        }

        // to be used when supporting multiple data wedges
        private void OnReceiveEnumerateScannerInfoIntentResult(Intent intent)
        {
            List<ScannerModel> scannerModelList = new List<ScannerModel>();
            IJavaObject scannerList = intent.GetSerializableExtra(INTENT_RESULT_ENUMERATE_SCANNER);
            //C# is dump in casting a list of Java IO object to anything else
            var bundleList = scannerList.JavaCast<JavaList<Bundle>>();
            if ((bundleList != null) && bundleList.Size() > 0)
            {
                foreach (Bundle bundle in bundleList)
                {
                    string name = bundle.GetString("SCANNER_NAME");
                    bool connectionState = bundle.GetBoolean("SCANNER_CONNECTION_STATE");

                    string identifier = bundle.GetString("SCANNER_IDENTIFIER");
                    ScannerModel model = new ScannerModel(
                        identifier,
                        name,
                        connectionState);
                    if (model.Id == "INTERNAL_CAMERA")
                    {
                        model.IsMainCamera = true;
                    }
                    scannerModelList.Add(model);
                }

            }
            scannerModelList = scannerModelList.Where(x => x.ConnectionState).ToList();

            // we will use our default zxing view for camera scanning
            if (ScannersInfoReceived != null)
            {
                ScannersInfoReceived(this, scannerModelList);
            }
        }
    }
}