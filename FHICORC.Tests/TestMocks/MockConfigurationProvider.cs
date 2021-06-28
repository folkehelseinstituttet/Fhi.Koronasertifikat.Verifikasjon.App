using System;
using System.IO;
using FHICORC.Core.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockConfigurationProvider : IConfigurationProvider
    {
        public MockConfigurationProvider()
        {
        }

        public Stream GetConfiguration()
        {
            return null;
        }

        public string GetEnvironment()
        {
            return "Unittests";
        }
    }
}
