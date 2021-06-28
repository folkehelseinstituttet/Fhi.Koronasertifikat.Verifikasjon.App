#nullable enable
using System.Collections.Generic;
using Android.Content.PM;
using FHICORC.Droid.Services.ImagerService;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using Xamarin.Essentials;

namespace FHICORC.Droid.Services
{
    public class AndroidScannerFactory : IScannerFactory
    {
        private Dictionary<SupportedScanner, string> _supportedScannerPackages = new Dictionary<SupportedScanner, string>()
        {
            { SupportedScanner.DataWedge, "com.symbol.datawedge"}
        };

        private Dictionary<SupportedScanner, string> _supportedManufacturers = new Dictionary<SupportedScanner, string>()
        {
            { SupportedScanner.Newland, "Newland"}
        };

        private IImagerScanner _currentScanner { get; set; }
        public IImagerScanner? GetAvailableScanner()
        {
            PackageManager? pm = Android.App.Application.Context.PackageManager;
            foreach (var scanner in _supportedScannerPackages)
            {
                try
                {
                    pm?.GetPackageInfo(scanner.Value, PackageInfoFlags.Activities);
                    _currentScanner = new ZebraTechnologyScanner();
                    break;
                }
                catch (PackageManager.NameNotFoundException e)
                {
                }
            }

            foreach (var manufacturer in _supportedManufacturers)
            {
                if (DeviceInfo.Manufacturer == manufacturer.Value)
                {
                    _currentScanner = new NewlandScanner();
                    break;
                }
            }

            return _currentScanner;
        }

        public IScannerConfig? GetScannerConfig()
        {
            switch (_currentScanner)
            {
                case ZebraTechnologyScanner _:
                    var config = new ZebraScannerConfig()
                    {
                        IsUPCE0 = false,
                        IsUPCE1 = false
                    };
                    return config;
                default:
                    return null;
            }
        }
    }

    public enum SupportedScanner
    {
        DataWedge,
        Newland
    }
}