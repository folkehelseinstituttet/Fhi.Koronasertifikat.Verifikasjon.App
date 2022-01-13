using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.DecoderServices;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.WebServices;
using FHICORC.Models;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Mocks;
using FHICORC.Services.Repositories;
using FHICORC.Services.WebServices;
using FHICORC.ViewModels;
using FHICORC.ViewModels.Error;
using FHICORC.ViewModels.Menu;
using FHICORC.ViewModels.QrScannerViewModels;
using TinyIoC;
using FHICORC.Core.Services;
using FHICORC.Services.DataManagers;
using FHICORC.Core.Services.BusinessRules;
using FHICORC.Core.Services.Model.Converter;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;
using FHICORC.Core.Services.Repositories;
using FHICORC.Core.Services.DecoderService;

namespace FHICORC.Configuration
{
    public static class IoCContainer
    {
        private static TinyIoCContainer _container;

        static IoCContainer()
        {
            _container = new TinyIoCContainer();
            RegisterProfileService();
            RegisterServices();
            RegisterViewModels();
            
        }

        public static void RegisterViewModels()
        {
            _container.Register<LandingViewModel>();
            _container.Register<SplashViewModel>();
            _container.Register<QRScannerViewModel>();
            _container.Register<SuccessViewModel>();
            _container.Register<BaseErrorViewModel>();
            _container.Register<MenuPageViewModel>();
            _container.Register<QRScannerMenuViewModel>();
            _container.Register<QRScannerSettingsViewModel>();
            _container.Register<ScannerErrorViewModel>();
            _container.Register<ScanSuccessResultPopupViewModel>();
            _container.Register<ImagerScannerViewModel>();
            _container.Register<ScanEuVaccineResultViewModel>();
            _container.Register<ScanEuTestResultViewModel>();
            _container.Register<ScanEuRecoveryResultViewModel>();
            _container.Register<NoInternetViewModel>();
            _container.Register<AcceptTermsViewModel>();
        }

        private static void RegisterServices()
        {
            
            _container.Register<IPreferencesService, PreferencesService>();
            _container.Register<IDialogService, DialogService>();
            _container.Register<ISecureStorageService<PublicKeyStorageModel>, SecureStorageService <PublicKeyStorageModel>>();
            _container.Register<ISecureStorageService<string>, SecureStorageService<string>>();
            _container.Register<INavigationService, NavigationService>();
            _container.Register<INavigationTaskManager, NavigationTaskManager>();
            _container.Register<IPublicKeyRepository, MockPublicKeyRepository>();
            _container.Register<IConnectivityService, ConnectivityService>();
            _container.Register<IDateTimeService, DateTimeService>().AsSingleton();
            _container.Register<IPublicKeyStorageRepository, PublicKeyStorageRepository>();
            _container.Register<IBusinessRulesRepository, MockBusinessRulesRepository>();
            _container.Register<ITextService, TextService>();
            _container.Register<ITextRepository, TextRepository>();
            _container.Register<ICertificationService, CertificationService>();
            _container.Register<IPopupService, PopupService>();
            _container.Register<IDeviceFeedbackService, DeviceFeedbackService>();
            _container.Register<ISslCertificateService, SslCertificateService>().AsSingleton();
            _container.Register<IDgcValueSetTranslator, DigitalGreenValueSetDgcValueSetTranslator>();
            _container.Register<ITokenProcessorService, HcertTokenProcessorService>();
            _container.Register<IAssemblyService, AssemblyService>();
            _container.Register<ITimer, InterruptableTimer>();
            _container.Register<IRuleVerifierService, RuleVerifierService>();
            _container.Register<IRuleSelectorService, RuleSelectorService>();
            _container.Register<IValueSetService, ValueSetService>();
            _container.Register<IDigitalGreenValueSetTranslatorFactory, DigitalGreenValueSetTranslatorFactory>();
            _container.Register<IValueSetRepository, ValueSetRepository>();
            _container.Register<ISmartHealthCardRepository, SmartHealthCardRepository>();
            _container.Register<ICodingService, CodingService>();
            _container.Register<IUrlService, UrlService>();
        }

        //The services that need to be reset after the user logs out.
        public static void RegisterProfileService()
        {
            _container.Register<ISettingsService, SettingsService>().AsSingleton();
            _container.Register<IRestClient, RestClient>();
            _container.Register<IPublicKeyService, PublicKeyDataManager>().AsSingleton();
            _container.Register<IBusinessRulesService, BusinessRulesDataManager>().AsSingleton();
        }

        public static void UpdateDependencies(bool useMockServices)
        {
            if (useMockServices) return;


            _container.Register<IPublicKeyRepository, PublicKeyRepository>();
            _container.Register<IBusinessRulesRepository, BusinessRulesRepository>();

        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static void RegisterInterface<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>();
        }

        public static void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            _container.Register(instance);
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}