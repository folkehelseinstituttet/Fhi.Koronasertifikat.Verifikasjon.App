using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Views.Menu;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Base
{
    public abstract class BaseViewModel : BaseBindableObject
    {
        private bool _isLoading;
        protected static INavigationService _navigationService => IoCContainer.Resolve<INavigationService>();
        protected static ISettingsService _settingsService => IoCContainer.Resolve<ISettingsService>();

        public virtual ICommand BackCommand => new Command(async () => await ExecuteOnceAsync(_navigationService.PopPage));

        public ICommand HelpButtonCommand => new Command(async () => await ExecuteOnceAsync(async () => await _navigationService.PushPage(new HelpPage())));

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public LocaleService Strings => LocaleService.Current;

        protected void SetIsLoading(bool value)
        {
            IsLoading = value;
        }

        public virtual Task ExecuteOnReturn(object data)
        {
            return Task.FromResult(false);
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        protected async Task ExecuteOnceAsync(Func<Task> awaitableTask)
        {
            if (IsLoading) return;
            SetIsLoading(true);

            try
            {
                await awaitableTask();
            }
            finally
            {
                SetIsLoading(false);
            }
        }
    }
}
