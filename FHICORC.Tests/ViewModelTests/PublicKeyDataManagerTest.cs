using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using FHICORC.Configuration;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using FHICORC.Services;
using FHICORC.Services.DataManagers;
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

        [TestCase("2021-1-8 10:00")]
        [TestCase("2021-1-2 10:00:01")]
        public async Task TestFetchingPublicKey_CanCall(DateTime now)
        {
            var dateTimeService = IoCContainer.Resolve<IDateTimeService>() as MockDateTimeService;
            dateTimeService.Now = now;
            var publicKeyRepository = new MockPublicKeyRepository();
            var dataManager = new PublicKeyDataManager(
                publicKeyRepository,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                new MockPublicKeyStorageRepository(),
                IoCContainer.Resolve<INavigationTaskManager>());

            await dataManager.CheckAndFetchPublicKeyFromBackend();

            Assert.AreEqual(1, publicKeyRepository.GetPublicKeyCalledTimes);
        }

        [TestCase("2021-1-1 11:00")]
        [TestCase("2021-1-2 10:00:00")]
        public async Task TestFetchingPublicKey_CannotCall(DateTime now)
        {
            var dateTimeService = IoCContainer.Resolve<IDateTimeService>() as MockDateTimeService;
            dateTimeService.Now = now.ToUniversalTime();
            var publicKeyRepository = new MockPublicKeyRepository();
            var dataManager = new PublicKeyDataManager(
                publicKeyRepository,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                new MockPublicKeyStorageRepository(),
                IoCContainer.Resolve<INavigationTaskManager>());

            await dataManager.CheckAndFetchPublicKeyFromBackend();

            Assert.AreEqual(0, publicKeyRepository.GetPublicKeyCalledTimes);

        }
        [Test]
        public async Task TestFetchingPublicKeyFromSecureStorage()
        {
            var publicKeyRepository = new MockPublicKeyRepository();
            var dataManager = new PublicKeyDataManager(
                publicKeyRepository,
                IoCContainer.Resolve<ISettingsService>(),
                IoCContainer.Resolve<IDateTimeService>(),
                new MockPublicKeyStorageRepository(),
                IoCContainer.Resolve<INavigationTaskManager>());
            await dataManager.CheckAndFetchPublicKeyFromBackend();

            List<string> publickeys = await dataManager.GetPublicKeyByKid("test");

            Assert.AreEqual(publickeys.First(), "testpublickey");
        }
    }
}