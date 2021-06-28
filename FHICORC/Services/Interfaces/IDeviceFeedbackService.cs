using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FHICORC.Services.Interfaces
{
    public interface IDeviceFeedbackService
    {
        void Vibrate();
        void Vibrate(double durationMs);
        void PlaySound(string fileNameWithExtension);
        void PerformHapticFeedback(HapticFeedbackType type);
    }
}