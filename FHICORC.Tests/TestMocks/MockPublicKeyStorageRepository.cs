using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.Services.Model;
using FHICORC.Models;
using FHICORC.Services.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockPublicKeyStorageRepository : IPublicKeyStorageRepository
    {
        public Task DeletePublicKeyFromSecureStorage()
        {
            return Task.FromResult(true);
        }

        public Task<PublicKeyStorageModel> GetPublicKeyFromSecureStorage()
        {
            return Task.FromResult(new PublicKeyStorageModel()
            {
                PublicKeys = new List<PublicKeyDto>() { new PublicKeyDto() { Kid = "test", PublicKey = "testpublickey" } },
                LastFetchTimestamp = Convert.ToDateTime("2021-1-1 10:00")
            });
        }

        public Task SavePublicKeyToSecureStorage(PublicKeyStorageModel publicKeyStorageModel)
        {
            return Task.FromResult(true);
        }
    }
}
