using FHICORC.Core.Data;
using FHICORC.Core.Services.Interface;
using FHICORC.Models;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels.Menu;

namespace FHICORC.ViewModels.QrScannerViewModels
{
    public class QRScannerSettingsViewModel: SettingsPageViewModel
    {
        public QRScannerSettingsViewModel(
            IDialogService dialogService, 
            IPreferencesService preferencesService,
            ITextService textService,
            ISecureStorageService<PublicKeyStorageModel> secureStorageService,
            IPublicKeyService publicKeyService,
            IDateTimeService dateTimeService
            ) : base(dialogService, textService, preferencesService, secureStorageService, publicKeyService, dateTimeService)
        {
        }

        public override bool FromLandingPage => true;
    }
}
