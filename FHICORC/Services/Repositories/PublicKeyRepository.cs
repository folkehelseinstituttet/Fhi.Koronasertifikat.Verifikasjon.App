using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Core.Services.Repositories;

namespace FHICORC.Services.WebServices
{
    public class PublicKeyRepository : BaseRepository, IPublicKeyRepository
    {
        public PublicKeyRepository(
            IRestClient restClient,
            IUrlService urlService) : base(restClient, urlService)
        {
        }

        public async Task<ApiResponse<List<PublicKeyDto>>> GetPublicKey()
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.PublicKey);
            return await _restClient.Get<List<PublicKeyDto>>(url);
        }
    }
}
