using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FHICORC.Core.Services.Interface
{
    public interface ISslCertificateService
    {
        X509Certificate GetTrustedCertificate();
        void SetTrustedCertificate(Stream stream);
    }
}