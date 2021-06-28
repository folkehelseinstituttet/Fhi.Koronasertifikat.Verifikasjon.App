using System;
using System.Reflection;

namespace FHICORC.Core.Interfaces
{
    public interface IAssemblyService
    {
        string CertificatesFolderPath { get; }
        Assembly GetSharedFormsAssembly();
    }
}