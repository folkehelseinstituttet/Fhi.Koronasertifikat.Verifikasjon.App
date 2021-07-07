using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Tests.TestMocks;

namespace FHICORC.Tests.ServiceTests
{
    public class LocaleServiceTests
    {
        private Stream testStream;
        private LocaleService localeService = LocaleService.Current;

        [SetUp]
        public void SetUp()
        {
            IoCContainer.RegisterInterface<IPreferencesService, MockPreferencesService>();
            testStream = new MemoryStream(Encoding.UTF8.GetBytes(@"{""TEST_KEY"": ""TEST"", ""TEST_KEY_2"": ""TEST {0} TEST""}"));
        }

        [Test]
        public void Translate_KeyNotFoundReturnsKeyWithNotFoundSymbol()
        {
            string translated = localeService.Translate("ANOTHER_TEST_KEY");
            Assert.AreEqual("$ANOTHER_TEST_KEY$", translated);
        }

        [Test]
        public void Translate_KeyFoundReturnsValueForCurrentLocale()
        {
            localeService.LoadLocale("nb", testStream, false);
            string translated = localeService.Translate("TEST_KEY");
            Assert.AreEqual("TEST", translated);
        }

        [Test]
        public void Translate_WithArgsReturnsFormatted()
        {
            localeService.LoadLocale("nb", testStream, false);
            string translated = localeService.Translate("TEST_KEY_2", "TEST");
            Assert.AreEqual("TEST TEST TEST", translated);
        }

        [Test]
        public void GetLanguage_ReturnsCorrectLanguage()
        {
            localeService.LoadLocale("nb", testStream, false);
            Assert.AreEqual(LanguageSelection.Bokmal, LocaleService.Current.GetLanguage());
        }

        [Test]
        public void LoadLocale_InvalidJsonThrowsException()
        {
            Stream invalidStream = new MemoryStream(Encoding.UTF8.GetBytes("\"TEST_KEY\": \"TEST\" \"TEST_KEY_2\": \"TEST {0} TEST\""));
            Assert.Throws<JsonReaderException>(() =>
            {
                localeService.LoadLocale("nb", invalidStream, false);
            });
        }

        [Test]
        public void LoadLocale_OverwritesOldLocales()
        {
            Stream anotherStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"TEST_KEY\": \"TEST2\"}"));
            localeService.LoadLocale("nb", testStream, false);
            localeService.LoadLocale("en", anotherStream, false);
            Assert.AreEqual(LanguageSelection.English, LocaleService.Current.GetLanguage());
            Assert.AreEqual("TEST2", LocaleService.Current.Translate("TEST_KEY"));
        }
    }
}
