using Android.App;
using System.Linq;
using System.IO;
using System.Reflection;
using FHICORC.Core.Interfaces;

namespace FHICORC.Droid
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly string environmentVariable;

        public ConfigurationProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var attributes = assembly.GetCustomAttributes(true);

            var config = attributes.OfType<AssemblyConfigurationAttribute>().FirstOrDefault();

            environmentVariable = config?.Configuration;
        }

        public Stream GetConfiguration()
        {
            return Application.Context?.Assets?.Open($"appsettings.{environmentVariable}.json");
        }

        public string GetEnvironment()
        {
            return environmentVariable;
        }
    }
}