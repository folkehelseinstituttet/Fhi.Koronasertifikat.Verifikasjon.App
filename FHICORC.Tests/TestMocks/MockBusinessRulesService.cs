using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.BusinessRules;

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

        public ICollection<BusinessRule> GetBusinessRules()
        {
            return new List<BusinessRule>
            {
                new BusinessRule
                {
                    Identifier = "GR-NO-0001"
                },
                new BusinessRule
                {
                    Identifier = "GR-NO-0002"
                },
                new BusinessRule
                {
                    Identifier = "VR-NO-0001"
                },
                new BusinessRule
                {
                    Identifier = "TR-NO-0001"
                },
                new BusinessRule
                {
                    Identifier = "TR-NO-0002"
                },
                new BusinessRule
                {
                    Identifier = "RR-NO-0001"
                },
                new BusinessRule
                {
                    Identifier = "RR-NO-0002"
                },
                new BusinessRule
                {
                    Identifier = "RR-NO-0003"
                },
                new BusinessRule
                {
                    Identifier = "GR-NX-0001"
                },
                new BusinessRule
                {
                    Identifier = "VR-NX-0001"
                },
                new BusinessRule
                {
                    Identifier = "VR-NX-0002"
                },
                new BusinessRule
                {
                    Identifier = "TR-NX-0001"
                },
                new BusinessRule
                {
                    Identifier = "RR-NX-0001"
                },
                new BusinessRule
                {
                    Identifier = "RR-NX-0002"
                },
            };
        }
    }
}
