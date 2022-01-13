using NUnit.Framework;
using FHICORC.Core.Services.DecoderServices;
using FHICORC.Core.Services.Interface;
using FHICORC.Tests.TestMocks;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;
using FHICORC.Tests.Factories;
using System.Collections.Generic;
using System;
using FHICORC.Core.Services.Model.Exceptions;

namespace FHICORC.Tests.ServiceTests
{
    public class CertificationServiceTests
    {
        private readonly ICertificationService certificationService;

        private readonly MockRestClient restClient;
        private readonly MockSmartHealthCardRepository smartHealthCardRepository;

        public CertificationServiceTests()
        {
            restClient = new MockRestClient();
            smartHealthCardRepository = new MockSmartHealthCardRepository();
            certificationService = new CertificationService(
                new MockPublicKeyDataManager(),
                restClient,
                smartHealthCardRepository
            );
        }

        [Test]
        public void VerifySHCSignature_WithMatchingPublicKey_DoesNotThrowException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson();
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.DoesNotThrowAsync(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_WithoutMatchingSignature_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(kid: "not matching");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_WithoutPublicKey_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(x5c: "");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<MissingDataException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_WithoutMatchingPublicKey_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(x5c: "\"notmatching\"");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<Exception>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCIssuer_TrustedIssuer_DoesNotThrowException()
        {
            smartHealthCardRepository.GetIssuerTrustResponse.Trusted = true;
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.DoesNotThrowAsync(async () => await certificationService.VerifySHCIssuer(parts));
        }

        [Test]
        public void VerifySHCIssuer_NotTrustedIssuer_ThrowsException()
        {
            smartHealthCardRepository.GetIssuerTrustResponse.Trusted = false;
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<Exception>(async () => await certificationService.VerifySHCIssuer(parts));
        }
    }
}
