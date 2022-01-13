using System;
using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Core.Services.Repositories;

namespace FHICORC.Services.Repositories
{
    public class TextRepository : BaseRepository, ITextRepository
    {
        public TextRepository(
            IRestClient restClient,
            IUrlService urlService) : base(restClient, urlService)
        {
        }

        public async Task<ApiResponse<Stream>> GetTexts(string currentVersion)
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.Text);
            _restClient.RegisterLocalesRequestHeaders(currentVersion);
            ApiResponse<Stream> response = await _restClient.GetFileAsStreamAsync(url);
            _restClient.ClearLocalesRequestHeaders();
            return response;
        }
    }
}
