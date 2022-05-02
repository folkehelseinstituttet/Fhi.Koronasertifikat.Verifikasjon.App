using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Utils;
using FHICORC.Core.WebServices;
using FHICORC.Data;
using FHICORC.Models.DataModels;
using FHICORC.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHICORC.Services.DataManagers
{
    using RevocationBatchCache = AsyncLazy<IProducerConsumerCollection<RevocationBatch>>;

    public class RevocationBatchDataManager : IRevocationBatchService
    {
        private static readonly AutoResetEvent _revocationBatchFetchingCompleteEvent = new AutoResetEvent(false);
        private static int _fecthingGuard;
        private readonly IDateTimeService _dateTimeService;
        private readonly INavigationTaskManager _navigationTaskManager;
        private readonly TimeSpan _periodicFetchingInterval;
        private readonly IRepository<RevocationBatch> _revocationBatchRepository;
        private readonly IRepository<AppLastFetchingDates> _appLastFetchingDatesRepository;
        private readonly IRestClient _restClient;
        private RevocationBatchCache _cachedRevocationBatches;

        public RevocationBatchDataManager(
            ISettingsService settingsService,
            INavigationTaskManager navigationTaskManager,
            IDateTimeService dateTimeService,
            IRepository<RevocationBatch> revocationBatchRepository,
            IRepository<AppLastFetchingDates> appLastFetchingDatesRepository,
            IRestClient restClient
            )
        {
            _dateTimeService = dateTimeService;
            _navigationTaskManager = navigationTaskManager;
            _periodicFetchingInterval = TimeSpan.FromHours(settingsService.RevocationBatchPeriodicFetchingIntervalInHours);
            _revocationBatchRepository = revocationBatchRepository;
            _appLastFetchingDatesRepository = appLastFetchingDatesRepository;
            _restClient = restClient;
            _cachedRevocationBatches = InitializeRevocationBatchCache();
        }

        public async Task<IEnumerable<RevocationBatch>> GetRevocationBatchesFromCountry(string isoCode)
        {
            var revocationBatches = await GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(isoCode).ConfigureAwait(false);

            if (!revocationBatches.Any())
            {
                _ = Task.Run(() => FetchRevocationBatchesFromBackend(true)).ConfigureAwait(false);
                _revocationBatchFetchingCompleteEvent.WaitOne(TimeSpan.FromSeconds(5));
                return await GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(isoCode).ConfigureAwait(false);
            }
            else
                _ = Task.Run(() => FetchRevocationBatchesFromBackend(false)).ConfigureAwait(false);

            return revocationBatches;
        }

        private Task FetchRevocationBatchesFromBackend(bool forced) => ParallelizationUtils.CreateInterlockedTask(async () =>
        {
            try
            {
                var entity = (await _appLastFetchingDatesRepository.GetEntitiesAsync(x => x.Name.Equals(LastFetchNames.RevocationBatch)).ConfigureAwait(false)).FirstOrDefault() ?? new AppLastFetchingDates() { Name = LastFetchNames.RevocationBatch };
                var lastFetch = entity.LastFetch.GetValueOrDefault();

                if (forced && lastFetch.AddSeconds(30) > _dateTimeService.Now)
                    return;

                if (forced || lastFetch.Add(_periodicFetchingInterval) <= _dateTimeService.Now)
                {
                    var response = await _restClient.Get<List<RevocationBatch>>(Urls.URL_GET_REVOCATION_BATCH).ConfigureAwait(false);

                    if (response.IsSuccessfull && response.Data != null)
                    {
                        entity.LastFetch = _dateTimeService.Now;
                        _ = await _appLastFetchingDatesRepository.AddOrUpdateEntityAsync(entity).ConfigureAwait(false);
                        _ = await _revocationBatchRepository.DropEntityTableAsync().ConfigureAwait(false); // Drop table to cleanup old entries not part of the newest fetch
                        _ = await _revocationBatchRepository.CreateEntityTableAsync().ConfigureAwait(false); // Due to the table drop a table recreation is needed
                        _ = await _revocationBatchRepository.AddOrUpdateEntitiesAsync(response.Data).ConfigureAwait(false);
                        _cachedRevocationBatches = new RevocationBatchCache(() => new ConcurrentBag<RevocationBatch>(response.Data));
                    }
                    else
                        await _navigationTaskManager.HandlerErrors(response, true).ConfigureAwait(false);
                }
            }
            finally
            {
                _revocationBatchFetchingCompleteEvent.Set();
            }
        }, ref _fecthingGuard);

        private RevocationBatchCache InitializeRevocationBatchCache() => new RevocationBatchCache(async () => new ConcurrentBag<RevocationBatch>(await _revocationBatchRepository.GetEntitiesAsync(_ => true).ConfigureAwait(false)));

        private async Task<IEnumerable<RevocationBatch>> GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(string isoCode) => (await _cachedRevocationBatches.Value.ConfigureAwait(false)).Where(x => x.Country.Equals(isoCode));

    }
}
