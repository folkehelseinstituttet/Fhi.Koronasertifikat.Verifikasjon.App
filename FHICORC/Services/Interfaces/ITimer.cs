using System;
namespace FHICORC.Services.Interfaces
{
    public interface ITimer
    {
        double MsRemainingSeconds
        {
            get;
        }

        bool Enabled { get; set; }

        Action OnStop
        {
            get;
            set;
        }

        Action OnTimeElapsed
        {
            get;
            set;
        }

        void Start(double countFrom, double timerInterval);
    }
}
