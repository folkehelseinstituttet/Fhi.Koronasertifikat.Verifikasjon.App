using System.Collections.Generic;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IPublicKeyService
    {
        Task CheckAndFetchPublicKeyFromBackend();
        Task FetchPublicKeyFromBackend(bool handleErrorsSilently);
        Task<List<string>> GetPublicKeyByKid(string base64Kid);
    }
}