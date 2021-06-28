using System.Threading.Tasks;
using FHICORC.Configuration;
using FHICORC.Models;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Mocks;
using FHICORC.Tests.TestMocks;
using NUnit.Framework;
using Xamarin.Forms;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Data;

namespace FHICORC.Tests.NavigationTests
{
    //See navigation mocks details on https://github.com/jonathanpeppers/Xamarin.Forms.Mocks

    public class NavigationServiceTests
    {
        INavigationService _navigationService = new ExtendedNavigationService();

        //[SetUp]
        //public void SetUp()
        //{
        //    // Add the required services, to make
        //    // Application.Current = new App();
        //    // work without errors.
        //    //IoCContainer.RegisterInterface<ISettingsService, MockSettingsService>();
        //    //IoCContainer.RegisterInterface<IPreferencesService, PreferencesService>();
        //    //IoCContainer.RegisterInterface<IUserService, UserService>();
        //    //IoCContainer.RegisterInterface<IStatusBarService, MockStatusBarService>();
        //    //IoCContainer.RegisterInterface<IDialogService, MockDialogService>();
        //    //IoCContainer.RegisterInterface<IWalletService, MockWalletService>();
        //    //IoCContainer.RegisterInterface<INavigationService, ExtendedNavigationService>();
        //    //IoCContainer.RegisterInterface<ISecureStorageService<Certificate>, MockSecureStorageService<Certificate>>();
        //    //IoCContainer.RegisterInterface<IConnectivityService, MockConnectivityService>();
        //    NavigationTaskManager.SUCCESS_SHOWN_MILLISECONDS = 0;

        //    //Start app
        //    Xamarin.Forms.Mocks.MockForms.Init();
        //    Application.Current = new App();

        //    _navigationService = IoCContainer.Resolve<INavigationService>() as ExtendedNavigationService;
        //}

        //[Test]
        //public async Task Navigation_GetCertificateSuccessfull()
        //{
        //    ////Given that the service will return a success certificate
        //    //MockGetCertificateResponse(200);
        //    ////And that we have a navigation stack
        //    //await IoCContainer.Resolve<INavigationService>().OpenTabbar();

        //    ////Then the main page is show
        //    //Page mainPage = Application.Current.MainPage;
        //    //Page tabbedPage = mainPage.Navigation.NavigationStack.Last();
        //    //Assert.True(tabbedPage is MainTabbedPage);

        //    //Page tab1 = (tabbedPage as MainTabbedPage).Children[0] as NavigationPage;
        //    //Assert.True(tab1.Navigation.NavigationStack[0] is ResultListViewPage);

        //    //Page tab2 = (tabbedPage as MainTabbedPage).Children[1] as NavigationPage;
        //    //Assert.True(tab2.Navigation.NavigationStack[0] is RetrieveCertificatePage);

        //    ////And the current tab is the Result list view
        //    //Page currentTab = ((tabbedPage as MainTabbedPage).CurrentPage as NavigationPage).Navigation.NavigationStack[0];
        //    //Assert.True(currentTab is ResultListViewPage);

        //    ////When fetching a certificate
        //    //var resultListViewModel = IoCContainer.Resolve<ResultListViewModel>();
        //    //await resultListViewModel.RetrieveCertificate(); 
            
        //    ////Then the tab will be changed and a CertificatePage will be pushed.
        //    //var currentTabStack = ((tabbedPage as MainTabbedPage).CurrentPage as NavigationPage).Navigation.NavigationStack;
        //    //Assert.AreEqual(2, currentTabStack.Count);
        //    //Assert.True(currentTabStack[0] is RetrieveCertificatePage);
        //    //Assert.True(currentTabStack[1] is CertificatePage);

        //    //var navigationHistory = _navigationService.History;
        //    //Assert.True(navigationHistory[0].Equals(new NavigationRecord(NavType.OpenTabbar, typeof(MainTabbedPage))));
        //    //Assert.True(navigationHistory[2].Equals(new NavigationRecord(NavType.PushModal, typeof(SuccessPage))));
        //    //Assert.True(navigationHistory[3].Equals(new NavigationRecord(NavType.PopPage, typeof(SuccessPage))));
        //    //Assert.True(navigationHistory[5].Equals(new NavigationRecord(NavType.GoToTab, typeof(RetrieveCertificatePage))));
        //    //Assert.True(navigationHistory[6].Equals(new NavigationRecord(NavType.PushPage, typeof(CertificatePage))));

        //}

        //// Test No internet
        ////[Test]
        ////public async Task Navigation_GetCertificateNoInternet()
        ////{

        ////}

        //// Test failure cases, and check that the right errors are shown.
        ////[TestCase(201)]
        ////[TestCase(204)]
        ////[TestCase(400)]
        ////[TestCase(404)]
        ////[TestCase(500)]
        ////public async Task Navigation_GetCertificateFailure(int httpStatusCode)
        ////{
        ////}

        //private void MockGetCertificateResponse(int httpStatusCode)
        //{
        //    //IoCContainer.RegisterSingleton<ICertificateRepository, MockCertificateRepository>();
        //    //MockCertificateRepository repo = IoCContainer.Resolve<ICertificateRepository>() as MockCertificateRepository;
        //    //repo.CustomResponse = new ApiResponse<CertificateDto>("url")
        //    //{
        //    //    Data = new CertificateDto
        //    //    {
        //    //        Name = "First Last",
        //    //        Expiry = DateTime.Now.ToUniversalTime().AddDays(20),
        //    //        DateOfBirth = DateTime.Now.ToUniversalTime(),
        //    //        QrCodeToken = "qrCodeToken",
        //    //        CertificateType = CertificateType.TestResult
        //    //    },
        //    //    StatusCode = httpStatusCode
        //    //};
        //}
    }
}
