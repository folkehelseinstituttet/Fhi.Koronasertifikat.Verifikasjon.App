using FHICORC.Core.WebServices;

namespace FHICORC.Core.Services.Repositories
{
    public class BaseRepository
    {
        protected readonly IRestClient _restClient;
        protected readonly IUrlService _urlService;

        public BaseRepository(
            IRestClient restClient,
            IUrlService urlService)
        {
            _restClient = restClient;
            _urlService = urlService;
        }
    }
}
