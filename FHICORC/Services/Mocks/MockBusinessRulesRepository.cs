using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FHICORC.Services.Mocks
{
    public class MockBusinessRulesRepository : IBusinessRulesRepository
    {
        public async Task<ApiResponse<ICollection<BusinessRule>>> GetBusinessRules()
        {
            string jsonResponse = @"[{ ""id"": ""VR-NO-0001"", ""description"": ""Vaccine provider must be approved"", ""type"": ""Acceptance"", ""certificateType"": ""Vaccination"", ""version"": ""1.0.0"", ""logic"": { ""in"": [{ ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/20/1525"", ""EU/1/21/1529"" ]] } }, { ""id"": ""VR-NO-0002"", ""description"": ""At least 21 days (7 if COVID-19 has been contracted) must have passed after vaccine dose 1 of 1, or at least 7 days must have passed after vaccine dose 2 of 2"", ""type"": ""Acceptance"", ""certificateType"": ""Vaccination"", ""version"": ""1.0.0"", ""logic"": { ""if"": [{ ""and"": [{ ""==="": [{ ""var"": ""payload.v.0.dn"" }, 1] }, { ""==="": [{ ""var"": ""payload.v.0.sd"" }, 1] } ] }, { ""if"": [{ ""in"": [{ ""var"": ""payload.v.0.mp"" }, [ ""EU/1/20/1528"", ""EU/1/20/1507"", ""EU/1/21/1529"" ]] }, { ""after"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.v.0.dt"" }, 7, ""day""] } ] }, { ""after"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.v.0.dt"" }, 21, ""day""] } ] } ] }, { ""if"": [{ ""and"": [{ ""==="": [{ ""var"": ""payload.v.0.dn"" }, 2] }, { ""==="": [{ ""var"": ""payload.v.0.sd"" }, 2] } ] }, { ""after"": [{ ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.v.0.dt"" }, 7, ""day""] } ] }, false ] } ] } }, { ""id"": ""TR-NO-0001"", ""description"": ""Test based Corona passports are not admitted"", ""type"": ""Acceptance"", ""certificateType"": ""Test"", ""version"": ""1.0.0"", ""logic"": { ""if"": [false, false, false] } }, { ""id"": ""RR-NO-0001"", ""description"": ""At least 10 days and at most 180 days must have passed after positive test result"", ""type"": ""Acceptance"", ""certificateType"": ""Recovery"", ""version"": ""1.0.0"", ""logic"": { ""after"": [{ ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 180, ""day""] }, { ""plusTime"": [{ ""var"": ""external.validationClock"" }, 0, ""day""] }, { ""plusTime"": [{ ""var"": ""payload.r.0.fr"" }, 10, ""day""] } ] } } ]";
            var response = new ApiResponse<ICollection<BusinessRule>>(JsonConvert.DeserializeObject<ICollection<BusinessRule>>(jsonResponse), 200);
            return response;
        }
    }
}
