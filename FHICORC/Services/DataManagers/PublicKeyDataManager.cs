using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Models;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.DataManagers
{
    public class PublicKeyDataManager: IPublicKeyService
    {
        private ISettingsService _settingsService;
        private IDateTimeService _dateTimeService;
        private IPublicKeyStorageRepository _publicKeySecureStorageService;
        private IPublicKeyRepository _publicKeyRepository;
        private INavigationTaskManager _navigationTaskManager;

        private PublicKeyStorageModel _publicKeyStorageModel { get; set; } = new PublicKeyStorageModel();
        private TimeSpan _periodicFetchingInterval { get; set; }

        public PublicKeyDataManager(
            IPublicKeyRepository publicKeyRepository,
            ISettingsService settingsService, 
            IDateTimeService dateTimeService, 
            IPublicKeyStorageRepository publicKeySecureStorageService,
            INavigationTaskManager navigationTaskManager
            )
        {
            _settingsService = settingsService;
            _dateTimeService = dateTimeService;
            _publicKeySecureStorageService = publicKeySecureStorageService;
            _publicKeyRepository = publicKeyRepository;
            _periodicFetchingInterval = TimeSpan.FromHours(_settingsService.PublicKeyPeriodicFetchingIntervalInHours);
            _navigationTaskManager = navigationTaskManager;
        }

        public async Task CheckAndFetchPublicKeyFromBackend()
        {
            var publicKeyStorageModel = await _publicKeySecureStorageService.GetPublicKeyFromSecureStorage();
            if (publicKeyStorageModel == null 
                || (!publicKeyStorageModel?.PublicKeys.Any() ?? true ) 
                || publicKeyStorageModel?.LastFetchTimestamp.ToUniversalTime().Add(_periodicFetchingInterval) < _dateTimeService.Now)
            {
                await FetchPublicKeyFromBackend(publicKeyStorageModel?.PublicKeys.Any() ?? false);
            }
        }

        public async Task FetchPublicKeyFromBackend(bool handleErrorsSilently)
        {
            var publicKeyStorageModel = await _publicKeySecureStorageService.GetPublicKeyFromSecureStorage();
            ApiResponse<List<PublicKeyDto>> response = await _publicKeyRepository.GetPublicKey();
            await _navigationTaskManager.HandlerErrors(response, handleErrorsSilently);
            if (response.Data != null && response.IsSuccessfull)
            {
                if (publicKeyStorageModel == null)
                {
                    publicKeyStorageModel = new PublicKeyStorageModel();
                }
                publicKeyStorageModel.PublicKeys = response.Data;
                publicKeyStorageModel.LastFetchTimestamp = _dateTimeService.Now;
                await _publicKeySecureStorageService.SavePublicKeyToSecureStorage(publicKeyStorageModel);
            }
            _publicKeyStorageModel = publicKeyStorageModel ?? new PublicKeyStorageModel();
        }

        public async Task<List<string>> GetPublicKeyByKid(string base64Kid)
        {
            var pks = _publicKeyStorageModel.PublicKeys.Where(x => x.Kid == base64Kid).Select(x => x.PublicKey);
            if ((pks.Count() < 1 && _publicKeyStorageModel?.LastFetchTimestamp.ToUniversalTime().Add(_periodicFetchingInterval) < _dateTimeService.Now)
                || !_publicKeyStorageModel.PublicKeys.Any())
            {
                await FetchPublicKeyFromBackend(true);
                pks = _publicKeyStorageModel.PublicKeys.Where(x => x.Kid == base64Kid).Select(x => x.PublicKey);
            }
            return pks.ToList();
        }
    }
}