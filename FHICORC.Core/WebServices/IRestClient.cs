using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FHICORC.Core.WebServices
{
    public interface IRestClient
    {
        bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors);
        Task<ApiResponse<T>> Get<T>(string uri);
        Task<ApiResponse> Post(string url);
        Task<ApiResponse<T>> Post<T>(object body, string url);
        Task<ApiResponse<Stream>> GetFileAsStreamAsync(string url);
        Task<string> GetSimpleString(string url);
        void RegisterLocalesRequestHeaders(string versionNumber);
        void ClearLocalesRequestHeaders();
        void RegisterValuesetsRequestHeaders(string lastTimestamp);
        void ClearValuesetsRequestHeaders();
    }
}