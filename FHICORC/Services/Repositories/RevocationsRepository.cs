using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;
using FHICORC.Models;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class RevocationsRepository : BaseRepository, IRevocationsRepository
    {
        public async Task<ApiResponse<ICollection<Batches>>> GetBatches()
        {
            string url = Urls.URL_GET_REVOCATIONS;
            return await _restClient.Get<ICollection<Batches>>(url);
        }
    }
}
