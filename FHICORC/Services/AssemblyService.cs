using System;
using System.Reflection;
using FHICORC.Core.Interfaces;

namespace FHICORC.Services
{
    public class AssemblyService : IAssemblyService
    {
        public string CertificatesFolderPath => "FHICORC.Certs";

        public Assembly GetSharedFormsAssembly()
        {
            return typeof(App).GetTypeInfo().Assembly;
        }
    }
}
