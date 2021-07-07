using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;

namespace FHICORC.Tests.TestMocks
{
    public class MockRestClient : IRestClient
    {
        public bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public Task<ApiResponse<T>> Get<T>(string uri)
        {
            return Task.FromResult(new ApiResponse<T>((ApiResponse) null));
        }

        public Task<ApiResponse> Post(string url)
        {
            return Task.FromResult(new ApiResponse());
        }

        public Task<ApiResponse<T>> Post<T>(object body, string url)
        {
            return Task.FromResult(new ApiResponse<T>((ApiResponse) null));
        }

        public Task<ApiResponse<Stream>> GetFileAsStreamAsync(string url)
        {
            return Task.FromResult(new ApiResponse<Stream>((ApiResponse) null));
        }

        public Task RegisterAccessTokenHeader()
        {
            return Task.FromResult(true);
        }

        public void ClearAccessTokenHeader()
        {
            
        }

        public Task RefreshAccessToken(string refreshToken)
        {
            return Task.FromResult(true);
        }

        public Task RegisterForceToken()
        {
            return Task.FromResult(true);
        }

        public void RegisterLocalesRequestHeaders(string versionNumber)
        {
        }

        public void ClearLocalesRequestHeaders()
        {
        }

        public void RegisterValuesetsRequestHeaders(string lastTimestamp)
        {
        }

        public void ClearValuesetsRequestHeaders()
        {
        }
    }
}