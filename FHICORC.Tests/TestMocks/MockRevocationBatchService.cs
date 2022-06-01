using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FHICORC.Tests.TestMocks
{
    public class MockRevocationBatchService : IRevocationBatchService
    {
        public Task FetchRevocationBatchesFromBackend(bool forced)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RevocationBatch>> GetRevocationBatchesFromCountry(string isoCode)
        {
            throw new NotImplementedException();
        }
    }
}
