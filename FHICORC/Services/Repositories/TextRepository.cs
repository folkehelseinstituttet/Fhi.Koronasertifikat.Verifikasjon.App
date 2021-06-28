using System;
using System.IO;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class TextRepository : BaseRepository, ITextRepository
    {
        public async Task<ApiResponse<Stream>> GetTexts(string currentVersion)
        {
            string url = Urls.URL_GET_TEXTS;
            _restClient.RegisterLocalesRequestHeaders(currentVersion);
            ApiResponse<Stream> response = await _restClient.GetFileAsStreamAsync(url);
            _restClient.ClearLocalesRequestHeaders();
            return response;
        }
    }
}
