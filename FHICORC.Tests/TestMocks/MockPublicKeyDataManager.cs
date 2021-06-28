using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Tests.TestMocks
{
    public class MockPublicKeyDataManager: IPublicKeyService
    {
        public Task CheckAndFetchPublicKeyFromBackend()
        {
            return Task.CompletedTask;
        }

        public Task FetchPublicKeyFromBackend(bool handleErrorsSilently)
        {
            return Task.CompletedTask;
        }

        public Task<List<string>> GetPublicKeyByKid(string base64Kid)
        {
            return (Task<List<string>>) Task.CompletedTask;
        }
    }
}