using System;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Enums;
using FHICORC.Models;
using FHICORC.Services;

namespace FHICORC.Tests.NavigationTests
{
    public class MockNavigationTaskManager : INavigationTaskManager
    {
        public MockNavigationTaskManager()
        {
        }

        public Task HandlerErrors(ApiResponse response, bool silently = false)
        {
            return Task.FromResult(new object());
        }

        public Task ShowSuccessPage(string message)
        {
            return Task.FromResult(new object());
        }
    }
}
