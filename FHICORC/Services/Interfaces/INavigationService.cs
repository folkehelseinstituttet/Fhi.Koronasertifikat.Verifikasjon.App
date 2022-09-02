using System;
using System.Threading.Tasks;
using FHICORC.Models;
using FHICORC.Enums;
using Xamarin.Forms;

namespace FHICORC.Services.Interfaces
{
    public interface INavigationService
    {
        int CurrentTab { get; set; }
        public Page FindCurrentPage();
        public Task<Page> FindCurrentPageAsync(); //Use this method if you need to be sure all PushPage actions have been completed first.

        //Set the main page
        void OpenLandingPage();

        //Push page
        Task PushPage(Page page, bool animated = true, PageNavigationStyle style = PageNavigationStyle.PushInNavigation, object data = null);

        //Pop page
        Task PopPage(bool animated = true);
        Task PopPage();
        void OnDidDismissSheetPageOnIOS();
        void RemoveInstancesOfPageFromStackBelowTopPage(Type pageType);

        //Others
        Task GoToErrorPage(ErrorPageModel data);
        void SetStatusBar(Color backgroundColor, Color textColor);
        Task OpenAcceptTermsPage();
        //Set the load screen when app opens
        void InitialDataLoadPage();
    }
}