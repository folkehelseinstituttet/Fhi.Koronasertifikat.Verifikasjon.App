using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class BusinessRulesRepository : BaseRepository, IBusinessRulesRepository
    {
        public async Task<ApiResponse<ICollection<BusinessRule>>> GetBusinessRules()
        {
            string url = Urls.URL_GET_BUSINESS_RULES;
            return await _restClient.Get<ICollection<BusinessRule>>(url);
        }
    }
}
