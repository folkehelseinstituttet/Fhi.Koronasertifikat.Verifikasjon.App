using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Repositories;

namespace FHICORC.Services.WebServices
{
    public class PublicKeyRepository : BaseRepository, IPublicKeyRepository
    {
        public async Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey()
        {
            string url = Urls.URL_GET_PUBLIC_KEY;
            return await _restClient.Get<List<PublicKeyDto>>(url);
        }
    }
}
