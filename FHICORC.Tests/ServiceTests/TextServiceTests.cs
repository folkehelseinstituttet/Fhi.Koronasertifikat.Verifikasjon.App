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

namespace FHICORC.Tests.ServiceTests
{
    public class TextServiceTests
    {
        private ITextService textService;

        [OneTimeSetUp]
        public void SetUp()
        {
            textService = new TextService(new MockPreferencesService(), new Mock<ITextRepository>().Object, new MockDateTimeService(), new MockNavigationTaskManager());
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
    }
}
