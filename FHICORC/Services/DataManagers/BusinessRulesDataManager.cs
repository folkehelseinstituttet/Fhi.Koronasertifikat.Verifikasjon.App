using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.WebServices;
using FHICORC.Data;
using FHICORC.Services.Interfaces;
using Newtonsoft.Json;

namespace FHICORC.Services.DataManagers
{
    public class BusinessRulesDataManager : IBusinessRulesService
    {
        private IPreferencesService _preferencesService;
        private IDateTimeService _dateTimeService;
        private INavigationTaskManager _navigationTaskManager;
        private ISettingsService _settingsService;
        private IBusinessRulesRepository _businessRulesRepository;

        private const Environment.SpecialFolder BUSINESS_RULES_FILE_DIRECTORY = Environment.SpecialFolder.Personal;
        private const string BUSINESS_RULES_FILE_NAME = "business_rules.json";

        private TimeSpan _periodicFetchingInterval { get; set; }

        private ICollection<BusinessRule> businessRules;

        public BusinessRulesDataManager(
            ISettingsService settingsService,
            IDateTimeService dateTimeService,
            INavigationTaskManager navigationTaskManager,
            IBusinessRulesRepository businessRulesRepository,
            IPreferencesService preferencesService
            )
        {
            _settingsService = settingsService;
            _dateTimeService = dateTimeService;
            _businessRulesRepository = businessRulesRepository;
            _preferencesService = preferencesService;
            _periodicFetchingInterval = TimeSpan.FromHours(_settingsService.PublicKeyPeriodicFetchingIntervalInHours);
            _navigationTaskManager = navigationTaskManager;
        }

        public async Task CheckAndFetchBusinessRulesFromBackend()
        {
            long lastTimeFetchedRules = _preferencesService.GetUserPreferenceAsLong(PreferencesKeys.LAST_TIME_FETCHED_RULES);
            if (new DateTime(lastTimeFetchedRules).ToUniversalTime().Add(_periodicFetchingInterval) < _dateTimeService.Now || true)
            {
                await FetchBusinessRulesFromBackend(!(lastTimeFetchedRules == 0));
            }
        }

        public async Task FetchBusinessRulesFromBackend(bool handleErrorsSilently)
        {
            ApiResponse<ICollection<BusinessRule>> response = await _businessRulesRepository.GetBusinessRules();
            await _navigationTaskManager.HandlerErrors(response, handleErrorsSilently);
            if (response.Data != null && response.IsSuccessfull)
            {
                SaveBusinessRulesFile(response.Data);
                _preferencesService.SetUserPreference(PreferencesKeys.LAST_TIME_FETCHED_RULES, _dateTimeService.Now.Ticks);
            }
        }

        private ICollection<BusinessRule> ReadBusinessRules()
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(BUSINESS_RULES_FILE_DIRECTORY), BUSINESS_RULES_FILE_NAME);
                var text = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<ICollection<BusinessRule>>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ICollection<BusinessRule> GetBusinessRules()
        {
            if (businessRules == null)
                businessRules = ReadBusinessRules();
            return businessRules;
        }

        private void SaveBusinessRulesFile(ICollection<BusinessRule> rulesText)
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(BUSINESS_RULES_FILE_DIRECTORY), BUSINESS_RULES_FILE_NAME);
                File.WriteAllText(path, JsonConvert.SerializeObject(rulesText));
            }
            catch (Exception)
            {
            }
        }
    }
}
