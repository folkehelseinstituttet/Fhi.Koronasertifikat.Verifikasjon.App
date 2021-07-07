using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Services.WebServices;
using FHICORC.Tests.NavigationTests;
using FHICORC.Tests.TestMocks;
using FHICORC.Core.Data;
using System.IO;
using System.Text;
using FHICORC.Data;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;

namespace FHICORC.Tests.ServiceTests
{
    public class TextServiceTests
    {
        private ITextService textService;
        private IPreferencesService preferencesService;

        private const Environment.SpecialFolder FILE_DIRECTORY = Environment.SpecialFolder.Personal;

        [OneTimeSetUp]
        public void SetUp()
        {
            preferencesService = new MockPreferencesService();
            textService = new TextService(preferencesService, new Mock<ITextRepository>().Object, new MockDateTimeService(), new MockNavigationTaskManager());
            IoCContainer.RegisterInterface<ISettingsService, MockSettingsService>();
            IoCContainer.RegisterInterface<IPreferencesService, MockPreferencesService>();
        }

        [Test]
        public void SetLocale_LocaleIsSet()
        {
            textService.SetLocale("en");
            Assert.AreEqual(LanguageSelection.English, LocaleService.Current.GetLanguage());
        }

        [Test]
        public async Task LoadSavedLocales_LoadsSuccessfully()
        {
            await textService.LoadSavedLocales();
        }

        [Test]
        public void SetLocale_LocaleIsSetToEmbedded()
        {
            textService.SetLocale("en");
            Assert.True(LocaleService.Current.LoadedFromEmbeddedFile);
        }

        [TestCase("en", "1.8")]
        [TestCase("nb", "1.8")]
        public async Task SetLocale_LocaleIsSetToFetchedFile(string lang, string versionOfFetchedFile)
        {
            await SimulatePreviouslyDownloadedFilesFromBackend(lang, versionOfFetchedFile);
            textService.SetLocale(lang);
            Assert.False(LocaleService.Current.LoadedFromEmbeddedFile);
            Assert.NotNull(LocaleService.Current.GetClassValueForKey<string>("TEST_KEY"));
            Assert.NotNull(LocaleService.Current.GetClassValueForKey<string>("TEST_KEY2"));
        }

        [TestCase("en", "1.0")]
        [TestCase("nb", "1.0")]
        [TestCase("nb", "1.5")]
        [TestCase("en", "1.5")]
        public async Task SetLocale_LocaleIsSetToEmbeddedFileBecauseFetchedFileIsOld(string lang, string versionOfFetchedFile)
        {
            await SimulatePreviouslyDownloadedFilesFromBackend(lang, versionOfFetchedFile);
            textService.SetLocale(lang);
            Assert.True(LocaleService.Current.LoadedFromEmbeddedFile);
            Assert.Null(LocaleService.Current.GetClassValueForKey<string>("TEST_KEY"));
            Assert.Null(LocaleService.Current.GetClassValueForKey<string>("TEST_KEY2"));
        }

        private async Task SimulatePreviouslyDownloadedFilesFromBackend(string lang, string version)
        {
            byte[] bytes;
            Stream textFileStream = new MemoryStream(Encoding.UTF8.GetBytes(@"{""TEST_KEY"": ""TEST"", ""TEST_KEY2"": ""TEST2""}"));
            using var memoryStream = new MemoryStream();
            await textFileStream.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();
            var path = Path.Combine(Environment.GetFolderPath(FILE_DIRECTORY), $"{lang}_{version}.json");
            File.WriteAllBytes(path, bytes);

            preferencesService.SetUserPreference(PreferencesKeys.CURRENT_TEXT_VERSION, version);
            preferencesService.SetUserPreference(PreferencesKeys.LANGUAGE_SETTING, lang);
        }
    }
}
