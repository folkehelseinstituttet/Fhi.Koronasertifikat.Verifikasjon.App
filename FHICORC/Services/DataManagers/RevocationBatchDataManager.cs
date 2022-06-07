using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Data.Models;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Utils;
using FHICORC.Core.WebServices;
using FHICORC.Data;
using FHICORC.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHICORC.Services.DataManagers
{
    using RevocationBatchCache = AsyncLazy<IProducerConsumerCollection<RevocationBatch>>;

    public class RevocationBatchDataManager : IRevocationBatchService
    {
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
            IRestClient restClient)
        {
            _dateTimeService = dateTimeService;
            _navigationTaskManager = navigationTaskManager;
            _periodicFetchingInterval = TimeSpan.FromHours(settingsService.RevocationBatchPeriodicFetchingIntervalInHours);
            _revocationBatchRepository = revocationBatchRepository;
            _appLastFetchingDatesRepository = appLastFetchingDatesRepository;
            _restClient = restClient;
            _cachedRevocationBatches = InitializeRevocationBatchCache();
        }


        public Task<IEnumerable<RevocationBatch>> GetRevocationBatchesFromCountry(string isoCode) => GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(isoCode);

        public Task FetchRevocationBatchesFromBackend(bool forced) => ParallelizationUtils.CreateInterlockedTask(async () =>
        {
            var entity = (await _appLastFetchingDatesRepository.GetEntitiesAsync(x => x.Name.Equals(LastFetchNames.RevocationBatch)).ConfigureAwait(false)).FirstOrDefault() ?? new AppLastFetchingDates() { Name = LastFetchNames.RevocationBatch };
            var lastFetch = entity.LastFetch.GetValueOrDefault();
            if (forced || lastFetch.Add(_periodicFetchingInterval) <= _dateTimeService.Now)
            {
                string lastFetchDateString;
                if (lastFetch == new DateTime())
                    lastFetchDateString = "2021-06-01T00:00:00Z";
                else
                    lastFetchDateString = lastFetch.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                _restClient.RegisterCustomRequestHeaders(("LastDownloaded", lastFetchDateString));
                var response = await _restClient.Get<List<RevocationBatch>>(Urls.URL_GET_REVOCATION_BATCH).ConfigureAwait(false);

                if (response.IsSuccessfull && response.Data != null)
                {
                    entity.LastFetch = _dateTimeService.Now;
                    _ = await _revocationBatchRepository.AddOrUpdateEntitiesAsync(response.Data).ConfigureAwait(false);
                    _ = await _appLastFetchingDatesRepository.AddOrUpdateEntityAsync(entity).ConfigureAwait(false);
                    _cachedRevocationBatches = InitializeRevocationBatchCache();
                }
                else
                    await _navigationTaskManager.HandlerErrors(response, true).ConfigureAwait(false);
            }
        }, ref _fecthingGuard);

        private RevocationBatchCache InitializeRevocationBatchCache() => new RevocationBatchCache(async () => new ConcurrentBag<RevocationBatch>(await _revocationBatchRepository.GetEntitiesAsync(_ => true).ConfigureAwait(false)));

        private async Task<IEnumerable<RevocationBatch>> GetRevocationBatchesMatchingCountryFromRevocationBatchesCache(string isoCode) => (await _cachedRevocationBatches.Value.ConfigureAwait(false)).Where(x => x.CountryISO3166.Equals(isoCode));

    }
}
