using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;
using FHICORC.Core.Services.Repositories;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class BusinessRulesRepository : BaseRepository, IBusinessRulesRepository
    {
        public BusinessRulesRepository(
            IRestClient restClient,
            IUrlService urlService) : base(restClient, urlService)
        {
        }

        public async Task<ApiResponse<ICollection<BusinessRule>>> GetBusinessRules()
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.BusinessRules);
            return await _restClient.Get<ICollection<BusinessRule>>(url);
        }
    }
}
