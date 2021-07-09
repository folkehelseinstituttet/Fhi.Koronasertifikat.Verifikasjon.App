using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.WebServices;
using FHICORC.Data;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.WebServices
{
    public class ValueSetService : IValueSetService
    {
        private readonly IValueSetRepository _valueSetRepository;
        private readonly IPreferencesService _preferencesService;
        private readonly INavigationTaskManager _navigationTaskManager;
        private readonly IDateTimeService _dateTimeService;

        private const Environment.SpecialFolder ZIP_FILE_DIRECTORY = Environment.SpecialFolder.Personal;
        private const string ZIP_FILE_NAME = "valuesets.zip";

        public ValueSetService(
            IValueSetRepository valueSetRepository,
            IPreferencesService preferencesService,
            INavigationTaskManager navigationTaskManager,
            IDateTimeService dateTimeService)
        {
            _valueSetRepository = valueSetRepository;
            _preferencesService = preferencesService;
            _navigationTaskManager = navigationTaskManager;
            _dateTimeService = dateTimeService;
        }

        public async Task CheckFetchAndSaveLatestVersionOfValueSets()
        {
            long lastTimeFetchedValuesets = _preferencesService.GetUserPreferenceAsLong(PreferencesKeys.LAST_TIME_FETCHED_VALUESETS);
            int minTimeBetweenFetches = IoCContainer.Resolve<ISettingsService>().MinimumHoursBetweenTextFetches;
            if ((_dateTimeService.Now - new DateTime(lastTimeFetchedValuesets).ToUniversalTime()).TotalHours > minTimeBetweenFetches)
            {
                await FetchAndSaveLatestVersionOfValueSets(lastTimeFetchedValuesets);
            }
        }

        public async Task FetchAndSaveLatestVersionOfValueSets(long lastTimeFetchedValuesets)
        {
            Stream result = await FetchZipFileFromServer(lastTimeFetchedValuesets);
            if (result != null && result.Length != 0)
            {
                string path = await SaveZipFile(result);
                if (!string.IsNullOrEmpty(path))
                {
                    long timestamp = ExtractZipFile(path);
                    _preferencesService.SetUserPreference(PreferencesKeys.LAST_TIME_FETCHED_VALUESETS, timestamp);
                }
            }
        }

        private long ExtractZipFile(string path)
        {
            try
            {
                using var zipArchive = ZipFile.OpenRead(path);

                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    zipArchiveEntry.ExtractToFile(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), zipArchiveEntry.FullName), true);
                }
                return _dateTimeService.Now.Ticks;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async Task<string> SaveZipFile(Stream zipStream)
        {
            try
            {
                byte[] bytes;
                using var memoryStream = new MemoryStream();
                await zipStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
                var path = Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), ZIP_FILE_NAME);
                File.WriteAllBytes(path, bytes);
                return path;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private async Task<Stream> FetchZipFileFromServer(long lastTimestamp)
        {
            var lastTimestampDate = new DateTimeOffset(lastTimestamp, TimeSpan.Zero);
            ApiResponse<Stream> response = await _valueSetRepository.GetValueSets(lastTimestampDate.ToString("O"));
            if (response.StatusCode == 204)
                return Stream.Null;
            await _navigationTaskManager.HandlerErrors(response, true);
            return response.Data;
        }

        public Stream GetValueSet(string fileName)
        {
            return File.OpenRead(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), fileName));
        }
    }
}
