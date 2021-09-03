using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace FHICORC.Core.WebServices
{
    public class CertificateValidationHandler : DelegatingHandler
    {
        public IRestClient RestClient;
        
        public CertificateValidationHandler(IRestClient restClient)
        {
            InnerHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ValidateServerCertificate
            };
            RestClient = restClient;
        }
 
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //return RestClient.ServerCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors);
        }
 
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}