using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.WebServices;
using FHICORC.Data;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using SQLite;

namespace FHICORC.Services.DataManagers
{
    public class RevocationsDataManager : IRevocationsService
    {
        private IRevocationsRepository _revocationsRepository;
        private IPreferencesService _preferencesService;
        private ISettingsService _settingsService;
        private IDateTimeService _dateTimeService;
        private INavigationTaskManager _navigationTaskManager;

        private TimeSpan _periodicFetchingInterval { get; set; }

        private SQLiteAsyncConnection _connection;

        public RevocationsDataManager(
            IRevocationsRepository revocationsRepository,
            IPreferencesService preferencesService,
            ISettingsService settingsService,
            IDateTimeService dateTimeService,
            INavigationTaskManager navigationTaskManager,
            ISqlConnection sqlConnection
            )
        {
            _revocationsRepository = revocationsRepository;
            _preferencesService = preferencesService;
            _settingsService = settingsService;
            _dateTimeService = dateTimeService;
            _navigationTaskManager = navigationTaskManager;
            _periodicFetchingInterval = TimeSpan.FromHours(_settingsService.RevocationsFetchingIntervalInHours);
            _connection = sqlConnection.GetConnection();
            _connection.CreateTableAsync<Batches>();
        }

        public async Task CheckAndFetchRevocationsFromBackend()
        {
            long lastTimeFetchedRevocations = _preferencesService.GetUserPreferenceAsLong(PreferencesKeys.LAST_TIME_FETCHED_REVOCATIONS);
            if (new DateTime(lastTimeFetchedRevocations).ToUniversalTime().Add(_periodicFetchingInterval) < _dateTimeService.Now) 
            {
                await FetchRevocationsFromBackend(!(lastTimeFetchedRevocations == 0));
            }
        }

        public async Task FetchRevocationsFromBackend(bool handleErrorsSilently)
        {
            ApiResponse<ICollection<Batches>> response = await _revocationsRepository.GetBatches();

            await _navigationTaskManager.HandlerErrors(response, handleErrorsSilently);
            if (response.Data != null && response.IsSuccessfull)
            {
                SaveBatchesToDatabase(response.Data);
                _preferencesService.SetUserPreference(PreferencesKeys.LAST_TIME_FETCHED_REVOCATIONS, _dateTimeService.Now.Ticks);
            }
        }

        private void SaveBatchesToDatabase(ICollection<Batches> batch)
        {
            try
            {
                foreach (Batches b in batch)
                {
                    Batches batches = new Batches
                    {
                        BatchID = b.BatchID,
                        Expires = b.Expires,
                        Deleted = b.Deleted
                    };
                    _connection.InsertOrReplaceAsync(batches);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}