using System.Threading.Tasks;
using FHICORC.Core.Data;
using FHICORC.Data;
using FHICORC.Models;
using FHICORC.Services.Interfaces;

namespace FHICORC.Services.Repositories
{
    public class PublicKeyStorageRepository : IPublicKeyStorageRepository
    {

        private readonly ISecureStorageService<PublicKeyStorageModel> _secureStorageService;

        public PublicKeyStorageRepository(ISecureStorageService<PublicKeyStorageModel> secureStorageService)
        {
            _secureStorageService = secureStorageService;
        }

        public async Task DeletePublicKeyFromSecureStorage()
        {
            await _secureStorageService.Clear(SecureStorageKeys.PUBLIC_KEY);
        }

        public async Task<PublicKeyStorageModel> GetPublicKeyFromSecureStorage()
        {
            PublicKeyStorageModel publicKeyStorageModel = new PublicKeyStorageModel();
            if (await _secureStorageService.HasValue(SecureStorageKeys.PUBLIC_KEY))
            {
                publicKeyStorageModel = await _secureStorageService.GetSecureStorageAsync(SecureStorageKeys.PUBLIC_KEY);
            }

            return publicKeyStorageModel;
        }

        public async Task SavePublicKeyToSecureStorage(PublicKeyStorageModel publicKeyStorageModel)
        {
            await _secureStorageService.SetSecureStorageAsync(SecureStorageKeys.PUBLIC_KEY, publicKeyStorageModel);
        }
    }
}
