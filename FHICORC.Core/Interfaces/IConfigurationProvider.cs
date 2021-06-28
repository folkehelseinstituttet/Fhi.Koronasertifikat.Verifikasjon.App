using System.IO;

namespace FHICORC.Core.Interfaces
{
    public interface IConfigurationProvider
    {
        Stream GetConfiguration();

        string GetEnvironment();
    }
}
