using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Tests.TestMocks;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace FHICORC.Tests.UITests
{
    public class BaseUITests
    {
        public BaseUITests()
        {
            //See navigation mocks details on https://github.com/jonathanpeppers/Xamarin.Forms.Mocks
            MockForms.Init();

            IoCContainer.RegisterInterface<IConfigurationProvider, MockConfigurationProvider>();
            IoCContainer.RegisterInterface<ISettingsService, MockSettingsService>();
            IoCContainer.RegisterInterface<IStatusBarService, MockStatusBarService>();
            IoCContainer.RegisterInterface<IPreferencesService, MockPreferencesService>();
            IoCContainer.RegisterInterface<IRestClient, MockRestClient>();

            Application.Current = new App();
        }
    }
}
