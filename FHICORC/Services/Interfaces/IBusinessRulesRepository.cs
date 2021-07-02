using System.Threading.Tasks;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;

namespace FHICORC.Services.Interfaces
{
    public interface IBusinessRulesRepository
    {
        public Task<ApiResponse<BusinessRulesDto>> GetBusinessRules();
    }
}
