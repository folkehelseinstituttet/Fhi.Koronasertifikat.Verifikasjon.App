using FHICORC.Core.Interfaces;
using Xamarin.Essentials;

namespace FHICORC.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public bool HasInternetConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
