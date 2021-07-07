using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
    public class TextService : ITextService
    {
        private readonly IPreferencesService _preferencesService;
        private readonly ITextRepository _textRepository;
        private readonly INavigationTaskManager _navigationTaskManager;
        private readonly IDateTimeService _dateTimeService;

        private const string ZIP_FILE_NAME = "locales.zip";
        private const Environment.SpecialFolder ZIP_FILE_DIRECTORY = Environment.SpecialFolder.Personal;

        public TextService(IPreferencesService preferencesService,
            ITextRepository textRepository,
            IDateTimeService dateTimeService,
            INavigationTaskManager navigationTaskManager)
        {
            _preferencesService = preferencesService;
            _textRepository = textRepository;
            _dateTimeService = dateTimeService;
            _navigationTaskManager = navigationTaskManager;
        }

        public async Task LoadSavedLocales()
        {
            await LoadEmbeddedCopies();
            SetLocales();
        }

        public async Task LoadRemoteLocales()
        {
            long lastTimeFetchedTexts = _preferencesService.GetUserPreferenceAsLong(PreferencesKeys.LAST_TIME_FETCHED_TEXTS);
            int minTimeBetweenFetches = IoCContainer.Resolve<ISettingsService>().MinimumHoursBetweenTextFetches;
            if ((_dateTimeService.Now - new DateTime(lastTimeFetchedTexts).ToUniversalTime()).TotalHours > minTimeBetweenFetches)
            {
                await FetchAndSaveLatestVersionOfLocales();
                SetLocales();
            }
        }

        private void SetLocales()
        {
            string selectedLanguage = _preferencesService.GetUserPreferenceAsString(PreferencesKeys.LANGUAGE_SETTING);
            SetLocale(string.IsNullOrEmpty(selectedLanguage) ? "nb" : selectedLanguage);
        }

        private async Task LoadEmbeddedCopies()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TextService)).Assembly;
                Stream nbStream = assembly.GetManifestResourceStream("FHICORC.Locales.nb.json");
                Stream nnStream = assembly.GetManifestResourceStream("FHICORC.Locales.nn.json");
                Stream enStream = assembly.GetManifestResourceStream("FHICORC.Locales.en.json");
                Stream valuesetsStream = assembly.GetManifestResourceStream("FHICORC.Locales.valuesets.csv");
                string nbText = "";
                string nnText = "";
                string enText = "";
                string valuesetsText = "";
                using (var reader = new System.IO.StreamReader(nbStream))
                {
                    nbText = await reader.ReadToEndAsync();
                }
                using (var reader = new System.IO.StreamReader(nnStream))
                {
                    nnText = await reader.ReadToEndAsync();
                }
                using (var reader = new System.IO.StreamReader(enStream))
                {
                    enText = await reader.ReadToEndAsync();
                }
                using (var reader = new System.IO.StreamReader(valuesetsStream))
                {
                    valuesetsText = await reader.ReadToEndAsync();
                }
                
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), "valuesets.csv"), valuesetsText);

                File.WriteAllText(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), "nb.json"), nbText);
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), "nn.json"), nnText);
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), "en.json"), enText);
            }
            catch (Exception)
            {

            }
        }

        private async Task FetchAndSaveLatestVersionOfLocales()
        {
            string currentVersion = _preferencesService.GetUserPreferenceAsString(PreferencesKeys.CURRENT_TEXT_VERSION);
            Stream result = await FetchZipFileFromServer(
                String.IsNullOrEmpty(currentVersion)
                ? IoCContainer.Resolve<ISettingsService>().EmbeddedTextVersion
                : currentVersion);
            if (result != null && result.Length != 0)
            {
                string path = await SaveZipFile(result);
                if (!string.IsNullOrEmpty(path))
                {
                    string versionNumber = ExtractZipFile(path);
                    _preferencesService.SetUserPreference(PreferencesKeys.CURRENT_TEXT_VERSION, versionNumber);
                }
            }
            _preferencesService.SetUserPreference(PreferencesKeys.LAST_TIME_FETCHED_TEXTS, DateTime.Now.Ticks);
        }

        public void SetLocale(string isoCode)
        {
            string versionNumber = _preferencesService.GetUserPreferenceAsString(PreferencesKeys.CURRENT_TEXT_VERSION);
            FileStream localeFile;
            try
            {
                localeFile = File.OpenRead(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), $"{isoCode}_{versionNumber}.json"));
                LocaleService.Current.LoadLocale(isoCode, localeFile);
                _preferencesService.SetUserPreference(PreferencesKeys.LANGUAGE_SETTING, isoCode);
            }
            catch (Exception)
            {
                localeFile = File.OpenRead(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), $"{isoCode}.json"));
                LocaleService.Current.LoadLocale(isoCode, localeFile);
                _preferencesService.SetUserPreference(PreferencesKeys.LANGUAGE_SETTING, isoCode);
            }
        }

        public Stream GetDgcValueSet()
        {
            string versionNumber = _preferencesService.GetUserPreferenceAsString(PreferencesKeys.CURRENT_TEXT_VERSION);
            FileStream localeFile;
            try
            {
                localeFile = File.OpenRead(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), $"valuesets_{versionNumber}.csv"));
            }
            catch (Exception)
            {
                localeFile = File.OpenRead(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), $"valuesets.csv"));
            }

            return localeFile;
        }

        private string ExtractZipFile(string path)
        {
            try
            {
                using var zipArchive = ZipFile.OpenRead(path);
                string versionNumber = "";
                string versionNumberRegexPattern = @"(?![\\_\.])[\d\.\\_]+(?i)(?=.json)";
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    zipArchiveEntry.ExtractToFile(Path.Combine(Environment.GetFolderPath(ZIP_FILE_DIRECTORY), zipArchiveEntry.FullName), true);
                    var matches = Regex.Matches(zipArchiveEntry.FullName, versionNumberRegexPattern);
                    List<Match> matchesList = new List<Match>();
                    foreach (Match match in matches)
                    {
                        matchesList.Add(match);
                    }
                    versionNumber = string.Join(".", matchesList.Select(x => x.Value));
                }
                return versionNumber;
            }
            catch (Exception)
            {
                return "";
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

        private async Task<Stream> FetchZipFileFromServer(string currentVersion)
        {
            ApiResponse<Stream> response = await _textRepository.GetTexts(currentVersion);
            if (response.StatusCode == 204)
                return Stream.Null;
            await _navigationTaskManager.HandlerErrors(response, true);
            return response.Data;
        }
    }
}
