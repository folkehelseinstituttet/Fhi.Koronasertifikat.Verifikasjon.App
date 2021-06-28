using Xamarin.Essentials;

namespace FHICORC.Core.Interfaces
{
    public interface IConnectivityService
    {
        bool HasInternetConnection();
    }
}
