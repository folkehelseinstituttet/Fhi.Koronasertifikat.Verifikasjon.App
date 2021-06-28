using System;
using System.Windows.Input;
using FHICORC.Configuration;
using FHICORC.Core.Services.Interface;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Error
{
    public class NoInternetViewModel : BaseErrorViewModel
    {

        private readonly IPublicKeyService _publicKeyService;

        public NoInternetViewModel()
        {
            _publicKeyService = IoCContainer.Resolve<IPublicKeyService>();
        }

        public override ICommand OkButtonCommand =>
            new Command(async () =>
            {
                await _navigationService.PopPage();
                await _publicKeyService.FetchPublicKeyFromBackend(false);
            });

        public override ICommand BackCommand => new Command(async () => await _navigationService.PopPage());
    }
}
