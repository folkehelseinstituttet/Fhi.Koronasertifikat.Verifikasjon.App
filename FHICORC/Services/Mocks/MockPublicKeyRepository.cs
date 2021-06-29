using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Mocks
{
    public class MockPublicKeyRepository : IPublicKeyRepository
    {
        public MockPublicKeyRepository()
        {
        }

        public async Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey()
        {
            ApiResponse<List<PublicKeyDto>> result = new ApiResponse<List<PublicKeyDto>>("");
            result.Data = new List<PublicKeyDto>()
            {
                new PublicKeyDto()
                {
                    Kid = "2Rk3X8HntrI=",
                    PublicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAErdVc9a0bltR6jm1BPTA3u0cyJNYKuF1uRk8h7h04+XBRJ9kYHt+/oSDXwmWXKM6cECncmqaKz1D9UxO1FpdBdw=="
                }
            };
            result.StatusCode = 200;
            return result;
        }
    }
}
