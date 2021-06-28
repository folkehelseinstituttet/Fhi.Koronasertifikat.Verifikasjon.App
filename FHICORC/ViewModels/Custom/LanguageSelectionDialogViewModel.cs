using System.Windows.Input;
using FHICORC.Configuration;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Custom
{
    public class LanguageSelectionDialogViewModel : CustomDialogViewModel
    {

        public bool IsBokmalSelected { get; set; }
        public bool IsNynorskSelected { get; set; }
        public bool IsEnglishSelected { get; set; }
        public ICommand CloseCommand { get; set; }
        private readonly ITextService _textService;

        public string BokmalText => "SETTINGS_CHOOSE_LANGUAGE_NB".Translate();
        public string NynorskText => "SETTINGS_CHOOSE_LANGUAGE_NN".Translate();
        public string EnglishText => "SETTINGS_CHOOSE_LANGUAGE_EN".Translate();
        public string CancelButton => "CANCEL_BUTTON".Translate();

        public LanguageSelectionDialogViewModel(string title, string body, bool isCanceledOnTouchOutside, string okButtonText = null) : base(title, body, isCanceledOnTouchOutside, okButtonText)
        {
            _textService = IoCContainer.Resolve<ITextService>();
            LanguageSelection languageSelection = LocaleService.Current.GetLanguage();
            IsBokmalSelected = languageSelection == LanguageSelection.Bokmal;
            IsNynorskSelected = languageSelection == LanguageSelection.Nynorsk;
            IsEnglishSelected = languageSelection == LanguageSelection.English;
            CloseCommand = new Command(() => Notifier.Cancel());
        }

        new public void Complete()
        {
            if (IsEnglishSelected)
            {
                _textService.SetLocale("en");
            }
            if (IsNynorskSelected)
            {
                _textService.SetLocale("nn");
            }
            if (IsBokmalSelected)
            {
                _textService.SetLocale("nb");
            }
            Notifier.Complete();
        }
    }
}
