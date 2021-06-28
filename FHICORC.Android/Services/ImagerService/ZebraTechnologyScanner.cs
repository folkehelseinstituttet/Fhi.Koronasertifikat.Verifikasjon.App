using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Scanner;
using FHICORC.Services.Status;

namespace FHICORC.Droid.Services.ImagerService
{
    public class ZebraTechnologyScanner : IImagerScanner
    {
        private Context _context = null;
        public IImagerReceiver Receiver { get; private set; }
        private bool _bRegistered = false;
        private static string ACTION_DATAWEDGE_FROM_6_2 = "com.symbol.datawedge.api.ACTION";
        private static string EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
        private static string EXTRA_ENUMERATE_SCANNER = "com.symbol.datawedge.api.ENUMERATE_SCANNERS";
        private static string EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";
        private static string EXTRA_PROFILE_NAME = "FHICORCS_DW_PROFILE";
        public static string EXTRA_SOFT_SCAN = "com.symbol.datawedge.api.EXTRA_PARAMETER";
        public static string EXTRA_SOFT_SCAN_PARAM_ENABLE = "START_SCANNING";
        public static string EXTRA_SOFT_SCAN_PARAM_DISABLE = "STOP_SCANNING";
   
        private ScannerModel _selectedScanner { get; set; }
        public List<ScannerModel> AvailableScanners { get; set; }
        public bool IsEnabled { get; private set; } = false;

        public ZebraTechnologyScanner()
        {
            _context = Application.Context;
            Receiver = new DataWedgeReceiver();
            CreateProfile();
        }
          
        public void Disable()
        {
            if (null != Receiver && null != _context && _bRegistered)
            {
                // Unregister the broadcast receiver
                _context.UnregisterReceiver(Receiver as BroadcastReceiver);
                _bRegistered = false;
            }

            DisableProfile();
            IsEnabled = false;
        }

        public void SetSelectedScanner(ScannerModel scannerModel)
        {
            _selectedScanner = scannerModel;
        }

        public List<ScannerModel> GetAvailableScanners()
        {
            return AvailableScanners;
        }

        public void Enable()
        {
            if (IsEnabled)
            {
                return;
            }

            Intent intent = new Intent("ACTION_BAR_SCANCFG");
            intent.PutExtra("EXTRA_SCAN_MODE", 3);
            intent.PutExtra("EXTRA_SCAN_AUTOENT", 1);
            _context.SendBroadcast(intent);

            if (null != Receiver && null != _context)
            {
                // Register the broadcast receiver
                IntentFilter filter = new IntentFilter();
                filter.AddAction(DataWedgeReceiver.INTENT_BARCODE_ACTION);
                filter.AddAction(DataWedgeReceiver.INTENT_RESULT_ACTION);
                filter.AddAction(DataWedgeReceiver.INTENT_SOFT_SCAN_ACTION);
                filter.AddCategory(DataWedgeReceiver.INTENT_CATEGORY);
                _context.RegisterReceiver(Receiver as BroadcastReceiver, filter);
                _bRegistered = true;
            }

            GetScannerInfo();
            EnableProfile();
            IsEnabled = true;
        }
        
        public void SetConfig(IScannerConfig a_config)
        {
            ZebraScannerConfig config = (ZebraScannerConfig)a_config;

            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
            profileConfig.PutString("PROFILE_ENABLED", _bRegistered ? "true" : "false"); //  Seems these are all strings
            profileConfig.PutString("CONFIG_MODE", "UPDATE");
            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "false"); //  This is the default but never hurts to specify
            Bundle barcodeProps = new Bundle();
            barcodeProps.PutString("scanner_input_enabled", "true");
            barcodeProps.PutString("scanner_selection_by_identifier", _selectedScanner?.Id ?? "AUTO" );
            barcodeProps.PutString("decoder_ean8", config.IsEAN8 ? "true" : "false");
            barcodeProps.PutString("decoder_ean13", config.IsEAN13 ? "true" : "false");
            barcodeProps.PutString("decoder_code39", config.IsCode39 ? "true" : "false");
            barcodeProps.PutString("decoder_code128", config.IsCode128 ? "true" : "false");
            barcodeProps.PutString("decoder_upca", config.IsUPCA ? "true" : "false");
            barcodeProps.PutString("decoder_upce0", config.IsUPCE0 ? "true" : "false");
            barcodeProps.PutString("decoder_upce1", config.IsUPCE1 ? "true" : "false");
            barcodeProps.PutString("decoder_d2of5", config.IsD2of5 ? "true" : "false");
            barcodeProps.PutString("decoder_i2of5", config.IsI2of5 ? "true" : "false");
            barcodeProps.PutString("decoder_aztec", config.IsAztec ? "true" : "false");
            barcodeProps.PutString("decoder_pdf417", config.IsPDF417 ? "true" : "false");
            barcodeProps.PutString("decoder_qrcode", config.IsQRCode ? "true" : "false");

            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);
            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", Android.App.Application.Context.PackageName);      //  Associate the profile with this app
            appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }

        private void GetScannerInfo()
        {
            //Send enumeration command to DataWedge
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_ENUMERATE_SCANNER, "");
        }

        public void EnableProfile()
        {
             // Now configure that created profile to apply to our application
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
            profileConfig.PutString("PROFILE_ENABLED", "true"); //  Seems these are all strings
            profileConfig.PutString("CONFIG_MODE", "UPDATE");
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }

        private void DisableProfile()
        {
            //  Now configure that created profile to apply to our application
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
            profileConfig.PutString("PROFILE_ENABLED", "false"); //  Seems these are all strings
            profileConfig.PutString("CONFIG_MODE", "UPDATE");
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }

        private void CreateProfile()
        {
            string profileName = EXTRA_PROFILE_NAME;
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_CREATE_PROFILE, profileName);

            //  Now configure that created profile to apply to our application
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
            profileConfig.PutString("PROFILE_ENABLED", "true"); //  Seems these are all strings
            profileConfig.PutString("CONFIG_MODE", "UPDATE");
            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true"); //  This is the default but never hurts to specify
            Bundle barcodeProps = new Bundle();
            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);
            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", Android.App.Application.Context.PackageName);      //  Associate the profile with this app
            appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
            //  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
            
            profileConfig.Remove("PLUGIN_CONFIG");
            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");
            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", DataWedgeReceiver.INTENT_BARCODE_ACTION);
            intentProps.PutString("intent_delivery", "2"); //Broadcast
            intentConfig.PutBundle("PARAM_LIST", intentProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }

        private void SendDataWedgeIntentWithExtra(String action, String extraKey, Bundle extras)
        {
            Intent dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extras);
            _context.SendBroadcast(dwIntent);
        }

        private void SendDataWedgeIntentWithExtra(String action, String extraKey, String extraValue)
        {
            Intent dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extraValue);
            _context.SendBroadcast(dwIntent);
        }
    }
}