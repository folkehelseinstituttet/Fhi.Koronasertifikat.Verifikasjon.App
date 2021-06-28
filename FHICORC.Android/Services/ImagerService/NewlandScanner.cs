using System.Collections.Generic;
using Android.App;
using Android.Content;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Scanner;

namespace FHICORC.Droid.Services.ImagerService
{
    public class NewlandScanner : IImagerScanner
    {
        private Context _context = null;
        private bool _bRegistered = false;
        public IImagerReceiver Receiver { get; private set; }
        public List<ScannerModel> AvailableScanners { get; set; }
        public bool IsEnabled { get; private set; } = false;

        public NewlandScanner()
        {
            _context = Application.Context;
            Receiver = new NewlandReceiver();
        }

        public void Disable()
        {
            if (Receiver != null && null != _context && _bRegistered)
            {
                // Unregister the broadcast receiver
                _context.UnregisterReceiver(Receiver as BroadcastReceiver);
                _bRegistered = false;
            }

            IsEnabled = false;
        }

        public void SetSelectedScanner(ScannerModel scannerModel)
        {

        }
     
        public void Enable()
        {
            if (IsEnabled)
            {
                return;
            }

            if (null != Receiver && null != _context)
            {
                // Register the broadcast receiver
                IntentFilter filter = new IntentFilter();
                filter.AddAction(NewlandReceiver.INTENT_RESULT_ACTION);
                _context.RegisterReceiver(Receiver as BroadcastReceiver, filter);
                _bRegistered = true;
            }
            IsEnabled = true;
        }

        public void SetConfig(IScannerConfig config)
        {
            Intent intent = new Intent("ACTION_BAR_SCANCFG");
            intent.PutExtra("EXTRA_SCAN_MODE", 3);
            intent.PutExtra("EXTRA_SCAN_AUTOENT", 1);
            _context.SendBroadcast(intent);
        }
    }
}