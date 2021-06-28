using System.Threading.Tasks;
using FHICORC.Models;

namespace FHICORC.Services.Interfaces
{
    public interface IPublicKeyStorageRepository
    {
        Task SavePublicKeyToSecureStorage(PublicKeyStorageModel publicKeyStorageModel);
        Task<PublicKeyStorageModel> GetPublicKeyFromSecureStorage();
        Task DeletePublicKeyFromSecureStorage();
    }
}
