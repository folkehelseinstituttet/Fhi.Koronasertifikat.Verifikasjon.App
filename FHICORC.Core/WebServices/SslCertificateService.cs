using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Core.WebServices
{
    public class SslCertificateService : ISslCertificateService
    {
        private X509Certificate _trustedCert;

        public SslCertificateService()
        {
        }
        
        public X509Certificate GetTrustedCertificate()
        {
            return _trustedCert;
        }

        public void SetTrustedCertificate(Stream trustedCertStream)
        {
            if (trustedCertStream == null)
            {
                Debug.WriteLine($"{nameof(SetTrustedCertificate)} failure: given parameter {nameof(trustedCertStream)} was null");
                return;
            }

            try
            {
                var ms = new MemoryStream();
                trustedCertStream.CopyTo(ms);
                _trustedCert = new X509Certificate(ms.ToArray());

            }
            catch (Exception)
            {

            }
        }
    }
}