namespace FHICORC.Core.Interfaces
{
    public interface ISettingsService
    {
        bool UseMockServices { get; set; }

        string BaseUrl { get; }

        string Environment { get; }

        string TrustedSSLCertificateFileName { get; }

        string AuthorizationHeader { get; }

        string ApiVersion { get; }

        int DefaultTimeout { get; }

        string BuildString { get; }

        string VersionString { get; }

        double ScannerShownDurationMs { get; }

        double ScannerEuShownDurationMs { get; }

        int MinimumHoursBetweenTextFetches { get; }

        string ForceUpdateLink { get; }

        string EmbeddedTextVersion { get; }
        int PublicKeyPeriodicFetchingIntervalInHours { get; }
    }
}