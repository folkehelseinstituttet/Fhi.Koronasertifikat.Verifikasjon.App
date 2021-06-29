using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model.BusinessRules;

namespace FHICORC.Core.Services.Interface
{
    public interface IBusinessRulesService
    {
        Task CheckAndFetchBusinessRulesFromBackend();
        Task FetchBusinessRulesFromBackend(bool handleErrorsSilently);
        ICollection<BusinessRule> ReadBusinessRules();
    }
}
