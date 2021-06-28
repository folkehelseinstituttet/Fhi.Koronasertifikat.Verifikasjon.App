using System;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Enums;
using FHICORC.Services.Interfaces;
using FHICORC.Views;
using FHICORC.Models;
using FHICORC.Services.Navigation;

namespace FHICORC.Services
{
    public class NavigationTaskManager : INavigationTaskManager
    {
        private INavigationService _navigationService;
        public static int SUCCESS_SHOWN_MILLISECONDS = 1500;

        public NavigationTaskManager(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <param name="silently">Set to true if the user should not be notified of error, e.g. fetching texts.</param>
        /// <returns></returns>
        public async Task HandlerErrors(ApiResponse response, bool silently = false)
        {
            if (response.IsSuccessfull)
                return;

            if (_navigationService.FindCurrentPage() is BaseErrorPage)
                return;

            switch (response.ErrorType)
            {

                case ServiceErrorType.NoInternetConnection:
                    if (!silently)
                        await _navigationService.GoToErrorPage(Errors.NoInternetError);
                    break;
                case ServiceErrorType.Gone:
                    await _navigationService.GoToErrorPage(Errors.ForceUpdateRequiredError);
                    break;
                default:
                    if (!silently)
                        await _navigationService.GoToErrorPage(Errors.UnknownError);
                    break;
            }
        }
    }
}
