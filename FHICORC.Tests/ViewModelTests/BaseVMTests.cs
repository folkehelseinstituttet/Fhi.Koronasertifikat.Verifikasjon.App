using System;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Services.Interfaces;
using FHICORC.Tests.TestMocks;

namespace FHICORC.Tests.ViewModelTests
{
    public class BaseVMTests
    {
        public BaseVMTests()
        {
            IoCContainer.RegisterInterface<ISettingsService, MockSettingsService>();
            IoCContainer.RegisterInterface<INavigationService, MockNavigationService>();
            IoCContainer.RegisterInterface<IPreferencesService, MockPreferencesService>();
        }
    }
}
