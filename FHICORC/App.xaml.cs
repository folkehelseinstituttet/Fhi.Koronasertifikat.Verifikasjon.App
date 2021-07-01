using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Data;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.RootCheck;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FHICORC
{
    public partial class App : Application
    {
        private ISettingsService _settingsService => IoCContainer.Resolve<ISettingsService>();
        private readonly IDialogService _dialogService;
        private readonly IPreferencesService _preferencesService;
        private readonly ITextService _textService;
        private readonly INavigationService _navigationService;
        private IPublicKeyService _publicKeyDataManager;
        private IBusinessRulesService _businessRulesDataManager;

        public App()
        {
            System.Diagnostics.Debug.Print("Initializing app.");
            InitializeComponent();

            IoCContainer.UpdateDependencies(_settingsService.UseMockServices);

            _dialogService = IoCContainer.Resolve<IDialogService>();
            _preferencesService = IoCContainer.Resolve<IPreferencesService>();
            _textService = IoCContainer.Resolve<ITextService>();
            _navigationService = IoCContainer.Resolve<INavigationService>();
            _publicKeyDataManager = IoCContainer.Resolve<IPublicKeyService>();
            _businessRulesDataManager = IoCContainer.Resolve<IBusinessRulesService>();
            ConfigureApp();
        }

        public async void PerformRootCheck()
        {
            var consent = _preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.ROOTCHECK_CONSENT);
            if (consent)
            {
                // If the user user already have given their consent, there is no need to perform root checking.
                System.Diagnostics.Debug.Print("user has previously given consent using this app on a possible rooted device.");
                return;
            }

            var isDeviceRooted = RootCheck.Current.IsDeviceRooted();
            System.Diagnostics.Debug.Print("is device rooted: {0}", isDeviceRooted);

            if (!isDeviceRooted)
            {
                return;
            }

            var title = "ROOTCHECK_TITLE".Translate();
            var description = "ROOTCHECK_DESCRIPTION".Translate();
            var accept = "ROOTCHECK_PROCEED_BUTTON".Translate();
            var consentGiven = await _dialogService.ShowAlertWithoutTouchOutsideAsync(title, description, accept, null);

            if (consentGiven)
            {
                // User has given their consent that they are aware the app is running in an unsafe environment.
                _preferencesService.SetUserPreference(PreferencesKeys.ROOTCHECK_CONSENT, true);
            }
            else
            {
                // Redisplay alert if canceled via backbutton
                PerformRootCheck();
            }
        }

        protected override async void OnStart()
        {
            VersionTracking.Track();
            if (VersionTracking.IsFirstLaunchEver)
            {
                ClearAppData();
            }
            await _textService.LoadSavedLocales();
            _navigationService.OpenLandingPage();
            await _textService.LoadRemoteLocales();
            await _businessRulesDataManager.CheckAndFetchBusinessRulesFromBackend();
            await _publicKeyDataManager.CheckAndFetchPublicKeyFromBackend();
            base.OnStart();
            PerformRootCheck();
        }

        private void ClearAppData()
        {
            _preferencesService.ClearAllUserPreferences();
            SetDefaultUserPreferences();
            SecureStorage.RemoveAll();
        }

        private void SetDefaultUserPreferences()
        {
            _preferencesService.SetUserPreference(PreferencesKeys.SCANNER_VIBRATION_SETTING, true);
            _preferencesService.SetUserPreference(PreferencesKeys.SCANNER_SOUND_SETTING, true);
            _preferencesService.SetUserPreference(PreferencesKeys.BORDER_CONTROL_ON, false);
            _preferencesService.SetUserPreference(PreferencesKeys.DOMESTIC_CONTROL_ON, false);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await _publicKeyDataManager.CheckAndFetchPublicKeyFromBackend();
            PerformRootCheck();
        }

        private void ConfigureApp()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

        }
    }
}
