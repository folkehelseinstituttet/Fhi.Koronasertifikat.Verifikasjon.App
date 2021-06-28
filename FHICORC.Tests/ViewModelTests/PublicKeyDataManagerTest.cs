using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model;
using FHICORC.Core.WebServices;
using FHICORC.Models;
using FHICORC.Services;
using FHICORC.Services.DataManagers;
using FHICORC.Services.Interfaces;
using FHICORC.Tests.NavigationTests;
using FHICORC.Tests.TestMocks;

namespace FHICORC.Tests.ViewModelTests
{
    public class PublicKeyDataManagerTest
    {
        public PublicKeyDataManagerTest()
        {
            IoCContainer.RegisterInterface<ISettingsService, MockSettingsService>();
            IoCContainer.RegisterInterface<IDateTimeService, MockDateTimeService>();
            IoCContainer.RegisterInterface<INavigationTaskManager, MockNavigationTaskManager>();
        }
        [TestCase("2021-1-1 10:00", "2021-1-8 10:00")]
        [TestCase("2021-1-1 10:00", "2021-1-2 10:00:01")]
        public async Task TestFetchingPublicKey_CanCall(DateTime lastFetch, DateTime now)
        {
            var dateTimeService = IoCContainer.Resolve<IDateTimeService>() as MockDateTimeService;
            dateTimeService.Now = now;
            
            var publicKeySecureStorage = new Mock<IPublicKeyStorageRepository>();
            publicKeySecureStorage.Setup(x => x.GetPublicKeyFromSecureStorage()).ReturnsAsync(
                new PublicKeyStorageModel()
                {
                    LastFetchTimestamp = lastFetch
                });
            var publicKeyService = new Mock<IPublicKeyRepository>();
            publicKeyService.Setup(x => x.GetPublicKey()).ReturnsAsync(new ApiResponse<List<PublicKeyDto>>(new List<PublicKeyDto>()));

            var dataManager = new PublicKeyDataManager(
                publicKeyService.Object,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                publicKeySecureStorage.Object,
                IoCContainer.Resolve<INavigationTaskManager>());
            await dataManager.CheckAndFetchPublicKeyFromBackend();
            publicKeyService.Verify(x => x.GetPublicKey(), Times.Once);
        }

        [TestCase("2021-1-1 10:00", "2021-1-1 11:00")]
        [TestCase("2021-1-1 10:00", "2021-1-2 10:00:00")]
        public async Task TestFetchingPublicKey_CannotCall(DateTime lastFetch, DateTime now)
        {
            var dateTimeService = IoCContainer.Resolve<IDateTimeService>() as MockDateTimeService;
            dateTimeService.Now = now.ToUniversalTime();
            
            var publicKeySecureStorage = new Mock<IPublicKeyStorageRepository>();
            publicKeySecureStorage.Setup(x => x.GetPublicKeyFromSecureStorage()).ReturnsAsync(
                new PublicKeyStorageModel()
                {
                    PublicKeys = new List<PublicKeyDto>(){new PublicKeyDto() {Kid = "test", PublicKey = "test"}},
                    LastFetchTimestamp = lastFetch.ToUniversalTime()
                });
            var publicKeyService = new Mock<IPublicKeyRepository>();

            var dataManager = new PublicKeyDataManager(
                publicKeyService.Object,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                publicKeySecureStorage.Object,
                IoCContainer.Resolve<INavigationTaskManager>());
            await dataManager.CheckAndFetchPublicKeyFromBackend();
            publicKeyService.Verify(x => x.GetPublicKey(), Times.Never);

        }
        [Test]
        public async Task TestFetchingPublicKeyFromSecureStorage()
        {
            var publicKeySecureStorage = new Mock<IPublicKeyStorageRepository>();
            publicKeySecureStorage.Setup(x => x.GetPublicKeyFromSecureStorage()).ReturnsAsync(
                new PublicKeyStorageModel()
                {
                    PublicKeys = new List<PublicKeyDto>(){new PublicKeyDto() {Kid = "test", PublicKey = "testpublickey"}},
                    LastFetchTimestamp = DateTime.Now
                });
            var publicKeyService = new Mock<IPublicKeyRepository>();
            publicKeyService.Setup(x => x.GetPublicKey()).ReturnsAsync(
                new ApiResponse<List<PublicKeyDto>>("test"));

            var dataManager = new PublicKeyDataManager(
                publicKeyService.Object,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                publicKeySecureStorage.Object,
                IoCContainer.Resolve<INavigationTaskManager>());
            await dataManager.CheckAndFetchPublicKeyFromBackend();
            List<string> publickeys = await dataManager.GetPublicKeyByKid("test");
            Assert.AreEqual( publickeys.First(), "testpublickey");
        }
    }
}