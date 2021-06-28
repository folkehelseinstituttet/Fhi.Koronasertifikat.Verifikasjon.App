using FHICORC.Services;
using FHICORC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FHICORC.ViewModels.Base
{
    public class BaseScanViewModel : BaseViewModel
    {
        public ITimer Timer { get; protected set; }

        public string SecondsRemainingText => $"{closesInText} {Math.Truncate(Timer.MsRemainingSeconds / 1000)} {secondsText}";

        protected const double TimerInterval = 1000;
        protected static double _startCountFrom = _settingsService.ScannerShownDurationMs;

        protected string closesInText = "POPUP_CLOSES_IN".Translate();
        protected string secondsText = "POPUP_CLOSES_IN_2".Translate();

        public BaseScanViewModel(ITimer timer)
        {
            Timer = timer;
            Timer.OnStop = ClosePage;
            Timer.OnTimeElapsed = () => OnPropertyChanged(nameof(SecondsRemainingText));
            Timer.Start(_startCountFrom, TimerInterval);
        }

        public override Task InitializeAsync(object navigationData)
        {
            UpdateTimerTextTranslations();

            return base.InitializeAsync(navigationData);
        }

        protected void ClosePage()
        {
            Device.BeginInvokeOnMainThread(async () => { await _navigationService.PopPage(); });
        }

        protected void UpdateTimerTextTranslations()
        {
            closesInText = "POPUP_CLOSES_IN".Translate();
            secondsText = "POPUP_CLOSES_IN_2".Translate();
        }
    }
}
