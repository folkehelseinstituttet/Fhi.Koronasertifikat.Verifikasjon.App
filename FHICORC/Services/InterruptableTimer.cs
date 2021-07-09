using System;
using System.Timers;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services
{
    public class InterruptableTimer : ITimer
    {
        private const int MaxTimeout = 2000;

        private double _msRemainingSeconds;
        private long _lastActive;

        public readonly Timer _timer = new Timer();
        public double MsRemainingSeconds { get => _msRemainingSeconds; }
        public Action OnStop { get; set; }
        public Action OnTimeElapsed { get; set; }
        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        public InterruptableTimer()
        {
            _timer.Elapsed += TimerOnElapsed;
        }

        public void Start(double countFrom, double timerInterval)
        {
            _msRemainingSeconds = countFrom;
            _timer.Interval = timerInterval;
            _timer.Enabled = true;
            _lastActive = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_msRemainingSeconds <= 0 && _timer.Enabled)
            {
                _timer.Enabled = false;
                OnStop();

            }
            else
            {
                // Due to iOS suspending the app when entering background state
                // this callback will stop while iOS apps are in the background,
                // hence resulting in the timer not decrementing.
                //
                // This is solved by checking if a max allowed timeout is exceeded,
                // and subtracting the time since the app was last detected active.

                long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                long timeSinceLastActive = (currentTime - _lastActive) / 1000 * 1000;
                if (timeSinceLastActive > MaxTimeout)
                {
                    _msRemainingSeconds -= (double)timeSinceLastActive;
                }
                else
                {
                    _msRemainingSeconds -= _timer.Interval;
                }

                if (_msRemainingSeconds < 0)
                    _msRemainingSeconds = 0;

                _lastActive = currentTime;

                OnTimeElapsed();
            }

        }
    }
}
