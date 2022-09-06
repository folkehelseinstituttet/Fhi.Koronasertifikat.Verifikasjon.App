using System;
using System.Threading.Tasks;
using FHICORC.Enums;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using Xamarin.Forms;

namespace FHICORC.Tests.TestMocks
{
    public class MockNavigationService : INavigationService
    {
        public MockNavigationService()
        {
        }

        public int CurrentTab { get; set; }

        public Page FindCurrentPage()
        {
            return new Page();
        }

        public Task<Page> FindCurrentPageAsync()
        {
            return Task.FromResult(new Page());
        }

        public Task GoToErrorPage(ErrorPageModel data)
        {
            return Task.CompletedTask;
        }

        public void OnDidDismissSheetPageOnIOS()
        {
        }

        public Task OpenAcceptTermsPage()
        {
            return Task.CompletedTask;
        }

        public void OpenLandingPage()
        {
        }

        public Task OpenTabbar()
        {
            return Task.CompletedTask;
        }

        public void OpenVerifyPinCodePage()
        {
        }

        public Task PopPage(bool animated = true)
        {
            return Task.CompletedTask;
        }

        public Task PopPage()
        {
            return Task.CompletedTask;
        }

        public Task PushPage(Page page, bool animated = true, PageNavigationStyle style = PageNavigationStyle.PushInNavigation, object data = null)
        {
            return Task.CompletedTask;
        }

        public Task PushPageOutsideTabbar(Page page, bool animated = true, bool modal = false, object data = null)
        {
            return Task.CompletedTask;
        }

        public void RemoveInstancesOfPageFromStackBelowTopPage(Type pageType)
        {
        }

        public void SetStatusBar(Color backgroundColor, Color textColor)
        {
        }

        public void InitialDataLoadPage()
        {
        }
    }
}
