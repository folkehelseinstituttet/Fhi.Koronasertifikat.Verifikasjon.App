using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Services.Mocks;

namespace FHICORC.Tests.TestMocks
{
    public class MockBusinessRulesService : IBusinessRulesService
    {
        public MockBusinessRulesService()
        {
        }

        public Task CheckAndFetchBusinessRulesFromBackend()
        {
            return Task.CompletedTask;
        }

        public Task FetchBusinessRulesFromBackend(bool handleErrorsSilently)
        {
            return Task.CompletedTask;
        }

        public ICollection<BusinessRule> ReadBusinessRules()
        {
            var repo = new MockBusinessRulesRepository();
            var result = repo.GetBusinessRules().GetAwaiter().GetResult();

            return result.Data;
        }
    }
}
