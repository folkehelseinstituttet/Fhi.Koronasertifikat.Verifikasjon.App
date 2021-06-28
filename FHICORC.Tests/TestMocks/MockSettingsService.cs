using FHICORC.Core.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockSettingsService : ISettingsService
    {
        public MockSettingsService()
        {
        }

        public bool UseMockServices { get; set; } = true;

        public string BaseUrl => "Https://baseurl.dk";
        public string AddJwtToGoogleUrl => "";

        public string TrustedSSLCertificateFileName => "local.crt";

        public string AuthorizationHeader => "Test-Subscription-Value";

        public bool LogErrors = false;
        public bool ShouldLogErrors => LogErrors;
        public string AndroidAppCenterKey => "";
        public string iOSAppCenterKey => "";

        public int DefaultTimeout => 15;

        public int PressedToReleasedCloseInterval => 100;

        public bool IsScrenShotAllowed => false;

        public int JwkValidForHours => 1;

        public int TimeOutMinuteUntilReauthenticate => 30;
        public string OAuthClientId => "";
        public string OAuthScopes => "";
        public string OAuthAuthorizeUrl => "";
        public string OAuthRedirectUrl => "";
        public string OAuthTokenUrl => "";
        public string OAuthSigningCertificate => "";
        public double TimeOutMinuteUntilNextPassportFetch => 1;
        public double ScannerShownDurationMs => 60000;
        public double ScannerEuShownDurationMs => 120000;
        public int MinimumHoursBetweenTextFetches => 24;
        public string ForceUpdateLink => "https://play.google.com/store";

        public string ApiV = "v1";
        public string ApiVersion => ApiV;

        public string Build = "14";
        public string BuildString => Build;

        public string Version = "1.0.0";
        public string VersionString => Version;

        public string EmbeddedTextVersion => "2.0.0";
        public int PublicKeyPeriodicFetchingIntervalInHours => 24;

        public string Environment => "mock";
    }
}
