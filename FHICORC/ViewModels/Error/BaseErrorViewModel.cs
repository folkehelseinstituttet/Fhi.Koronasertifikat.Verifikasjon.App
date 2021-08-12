using FHICORC.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FHICORC.Models;
using FHICORC.Views.Menu;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Configuration;
using FHICORC.Utils;

namespace FHICORC.ViewModels.Error
{
    public class BaseErrorViewModel : BaseViewModel
    {
        public virtual ICommand OkButtonCommand => BackCommand;

        public string HeaderText { get; } = "APP_TITLE".Translate();

        private string _titleMessage;
        private string _subtitleMessage;
        private string _imageSource;
        private string _buttonTitle;
        private string _secondButtonTitle;
        private bool _hasSecondButton;

        public string ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                RaisePropertyChanged(() => ImageSource);
            }
        }
       
        public string ErrorTitle
        {
            get
            {
                return _titleMessage;
            }
            set
            {
                _titleMessage = value;
                RaisePropertyChanged(() => ErrorTitle);
            }
        }

        public string ErrorSubtitle
        {
            get
            {
                return _subtitleMessage;
            }
            set
            {
                _subtitleMessage = value;
                RaisePropertyChanged(() => ErrorSubtitle);
            }
        }

        public string ButtonTitle
        {
            get
            {
                return _buttonTitle;
            }
            set
            {
                _buttonTitle = value;
                RaisePropertyChanged(() => ButtonTitle);
            }
        }

        public string SecondButtonTitle
        {
            get
            {
                return _secondButtonTitle;
            }
            set
            {
                _secondButtonTitle = value;
                RaisePropertyChanged(() => SecondButtonTitle);
            }
        }

        public bool HasSecondButton
        {
            get
            {
                return _hasSecondButton;
            }
            set
            {
                _hasSecondButton = value;
                RaisePropertyChanged(() => HasSecondButton);
            }
        }

        public BaseErrorViewModel()
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ErrorPageModel data)
            {
                if (!string.IsNullOrEmpty(data.Title)) ErrorTitle = data.Title;
                if (!string.IsNullOrEmpty(data.Message)) ErrorSubtitle = data.Message;
                if (!string.IsNullOrEmpty(data.Image)) ImageSource = data.Image;
                if (!string.IsNullOrEmpty(data.ButtonTitle)) ButtonTitle = data.ButtonTitle;
                if (!HasSecondButton) HasSecondButton = data.HasSecondButton;
                if (!string.IsNullOrEmpty(data.SecondButtonTitle)) SecondButtonTitle = data.SecondButtonTitle;
            }

            return base.InitializeAsync(navigationData);
        }
    }
}
