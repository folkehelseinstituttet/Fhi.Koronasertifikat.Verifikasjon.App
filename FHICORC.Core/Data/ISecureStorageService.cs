using System;
using System.Threading.Tasks;

namespace FHICORC.Core.Data
{
    public interface ISecureStorageService<TValue>
    {
        Task<TValue> GetSecureStorageAsync(string key);

        Task SetSecureStorageAsync(string key, TValue value);

        Task<bool> HasValue(string key);

        Task<bool> Clear(string key);
    }

 
}
