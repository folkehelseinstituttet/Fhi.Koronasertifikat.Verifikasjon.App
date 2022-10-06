/*using FHICORC.Core.Data;
using FHICORC.Core.Data.Models;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FHICORC.Tests.RevocationTests
{
    using RevocationBatchCache = AsyncLazy<IProducerConsumerCollection<RevocationBatch>>;

    public class MockRevocationBatchDataManager : IRevocationBatchService
    {
        private readonly IRepository<RevocationBatch> _revocationBatchRepository;
        private RevocationBatchCache _cachedRevocationBatches;

        public MockRevocationBatchDataManager(IRepository<RevocationBatch> revocationBatchRepository)
        {
            _revocationBatchRepository = revocationBatchRepository;
            _cachedRevocationBatches = InitializeRevocationBatchCache();
        }


        public Task<IEnumerable<RevocationBatch>> GetRevocationBatchesFromCountry(string isoCode) => GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(isoCode);

        public async Task FetchRevocationBatchesFromBackend(bool forced) 
        {
            var response = JsonConvert.DeserializeObject<List<RevocationBatch>>(File.ReadAllText("TestData/RevocationDownloadReponse.json"));

            _ = await _revocationBatchRepository.AddOrUpdateEntitiesAsync(response).ConfigureAwait(false);
            _cachedRevocationBatches = InitializeRevocationBatchCache();

        }

        private RevocationBatchCache InitializeRevocationBatchCache() => new RevocationBatchCache(async () => new ConcurrentBag<RevocationBatch>(await _revocationBatchRepository.GetEntitiesAsync(_ => true).ConfigureAwait(false)));

        private async Task<IEnumerable<RevocationBatch>> GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(string isoCode) => (await _cachedRevocationBatches.Value.ConfigureAwait(false)).Where(x => x.CountryISO3166.Equals(isoCode));

    }
}
*/