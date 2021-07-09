using System.IO;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class ValueSetRepository : BaseRepository, IValueSetRepository
    {
        public async Task<ApiResponse<Stream>> GetValueSets(string lastTimestamp)
        {
            string url = Urls.URL_GET_VALUESETS;
            _restClient.RegisterValuesetsRequestHeaders(lastTimestamp);
            ApiResponse<Stream> response = await _restClient.GetFileAsStreamAsync(url);
            _restClient.ClearValuesetsRequestHeaders();
            return response;
        }
    }
}
