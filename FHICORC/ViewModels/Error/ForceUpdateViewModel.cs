using System;
using System.Windows.Input;
using FHICORC.Core.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Error
{
    public class ForceUpdateViewModel: BaseErrorViewModel
    {
        public ForceUpdateViewModel()
        {
        }

        public override ICommand OkButtonCommand =>
            new Command(async () => await Launcher.OpenAsync(_settingsService.ForceUpdateLink));

        public override ICommand BackCommand => new Command(() => { });
    }
}
