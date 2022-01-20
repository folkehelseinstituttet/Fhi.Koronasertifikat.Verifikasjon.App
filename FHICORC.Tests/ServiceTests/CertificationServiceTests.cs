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
        public void VerifySHCSignature_WithEmptyXCoordinate_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(x: "");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<ArgumentException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_NotEqualXCoordinate_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(x: "UIHmslmdrBoRHAnFoTLvuTf9KdY9Kw3JddnzRIJDfQw");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<ArgumentException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_WithEmptyYCoordinate_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(y: "");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<ArgumentException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_NotEqualYCoordinate_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(y: "xvwMv_9g1Sl8GAySbhlhqbZYQ9XcXJXCC-dktsNpJjA");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<ArgumentException>(async () => await certificationService.VerifySHCSignature(parts));
        }

        [Test]
        public void VerifySHCSignature_NotEqualXAndYCoordinate_ThrowsException()
        {
            restClient.GetSimpleStringResponse = SmartHealthCardFactory.CreateJwksJson(
                x: "UIHmslmdrBoRHAnFoTLvuTf9KdY9Kw3JddnzRIJDfQw",
                y: "xvwMv_9g1Sl8GAySbhlhqbZYQ9XcXJXCC-dktsNpJjA");
            JwsParts parts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());

            Assert.ThrowsAsync<InvalidSignatureException>(async () => await certificationService.VerifySHCSignature(parts));
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
