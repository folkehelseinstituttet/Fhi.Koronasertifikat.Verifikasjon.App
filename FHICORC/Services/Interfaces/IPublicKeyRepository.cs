using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;

namespace FHICORC.Services.Interfaces
{
    public interface IPublicKeyRepository
    {
        public Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey();
    }
}
