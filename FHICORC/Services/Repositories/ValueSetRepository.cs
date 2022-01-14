using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Core.Services.Repositories;

namespace FHICORC.Services.Repositories
{
    public class ValueSetRepository : BaseRepository, IValueSetRepository
    {
        public ValueSetRepository(
            IRestClient restClient,
            IUrlService urlService) : base(restClient, urlService)
        {
        }

        public async Task<ApiResponse<Stream>> GetValueSets(string lastTimestamp)
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.ValueSets);
            _restClient.RegisterValuesetsRequestHeaders(lastTimestamp);
            ApiResponse<Stream> response = await _restClient.GetFileAsStreamAsync(url);
            _restClient.ClearValuesetsRequestHeaders();
            return response;
        }
    }
}
