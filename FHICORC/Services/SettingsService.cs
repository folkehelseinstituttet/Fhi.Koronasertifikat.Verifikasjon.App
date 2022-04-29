using System;
using System.IO;
using Newtonsoft.Json.Linq;
using FHICORC.Core.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FHICORC.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly JObject _settings;

        public SettingsService()
        {
            UseMockServices = true;
        }

        public SettingsService(IConfigurationProvider configurationProvider)
        {
            using var reader = new StreamReader(configurationProvider.GetConfiguration());
            var json = reader.ReadToEnd();
            _settings = JObject.Parse(json);
            string environment = configurationProvider.GetEnvironment();
            UseMockServices = environment == "local";
            Environment = environment;
        }

        public string Environment { get; }

        public bool UseMockServices { get; set; }

        public string BaseUrl => GetSetting<string>(nameof(BaseUrl));

        public string TrustedSSLCertificateFileName => GetSetting<string>(nameof(TrustedSSLCertificateFileName));

        public string AuthorizationHeader => GetSetting<string>(nameof(AuthorizationHeader));

        public string ApiVersion => GetSetting<string>(nameof(ApiVersion));

        public int DefaultTimeout => GetSetting<int>(nameof(DefaultTimeout));

        public string BuildString => AppInfo.BuildString;

        public string VersionString => AppInfo.VersionString;

        public double ScannerShownDurationMs => GetSetting<double>(nameof(ScannerShownDurationMs));

        public double ScannerEuShownDurationMs => GetSetting<double>(nameof(ScannerEuShownDurationMs));

        public int MinimumHoursBetweenTextFetches => GetSetting<int>(nameof(MinimumHoursBetweenTextFetches));

        public string EmbeddedTextVersion => GetSetting<string>(nameof(EmbeddedTextVersion));

        public int PublicKeyPeriodicFetchingIntervalInHours => GetSetting<int>(nameof(PublicKeyPeriodicFetchingIntervalInHours));

        public int RevocationsFetchingIntervalInHours => GetSetting<int>(nameof(RevocationsFetchingIntervalInHours));

        public string ForceUpdateLink
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                    case Device.macOS:
                        return GetSetting<string>("AppStoreLink");
                    case Device.Android:
                    default:
                        return GetSetting<string>("GooglePlayLink");
                }
            }
        }

        private T GetSetting<T>(string key)
        {
            var value = _settings.SelectToken(key);

            if (value == null)
            {
                throw new InvalidOperationException($"Key '{key}' does not exist in current settings file.");
            }

            return value.Value<T>();
        }


    }
}