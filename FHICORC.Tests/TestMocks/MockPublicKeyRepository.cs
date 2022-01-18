using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockPublicKeyRepository : IPublicKeyRepository
    {
        public int GetPublicKeyCalledTimes = 0;

        public Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey()
        {
            GetPublicKeyCalledTimes += 1;
            return Task.FromResult(new ApiResponse<List<PublicKeyDto>>("test"));
        }
    }
}
