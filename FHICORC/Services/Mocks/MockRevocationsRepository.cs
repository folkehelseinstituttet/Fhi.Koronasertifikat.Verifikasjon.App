using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using Newtonsoft.Json;

namespace FHICORC.Services.Mocks
{
    public class MockRevocationsRepository : IRevocationsRepository
    {
        public async Task<ApiResponse<ICollection<Batches>>> GetBatches()
        {
            var jsonResponse = @" [{ ""BatchID"": ""699978cf - d2d4 - 4093 - 8b54 - ab2cf695d76d"", ""Expires"": ""2022 - 02 - 28T09: 05:43 + 01:00"", ""Deleted"": 0 }, { ""BatchID"": ""871cd2c6-e7ec-4387-bda6-556efb9b80bd"", ""Expires"": ""2022-02-28T09:05:43+01:00"", ""Deleted"": 1}]";
            var response = new ApiResponse<ICollection<Batches>>(JsonConvert.DeserializeObject<ICollection<Batches>>(jsonResponse), 200);
            return response;
        }
    }
}
