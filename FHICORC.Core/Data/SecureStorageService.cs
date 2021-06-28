
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FHICORC.Core.Data
{
    public class SecureStorageService<TValue> : ISecureStorageService<TValue>
    {

        public SecureStorageService()
        {
        }

        public async Task<TValue> GetSecureStorageAsync(string key)
        {
            var store = await SecureStorage.GetAsync(key);

            return store != default ? JsonConvert.DeserializeObject<TValue>(store) : default(TValue);
        }

        public async Task SetSecureStorageAsync(string key, TValue value)
        {
            await SecureStorage.SetAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<bool> HasValue(string key)
        {
            return await GetSecureStorageAsync(key) != null;
        }

        public async Task<bool> Clear(string key)
        {
            return await Task.FromResult<bool>(SecureStorage.Remove(key));
        }
    }
}