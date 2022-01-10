using NUnit.Framework;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Jws;
using System.Threading.Tasks;
using FHICORC.Tests.Factories;

namespace FHICORC.Tests.Models
{
    public class JwsPartsTests
    {
        private readonly JwsParts jwsParts; 

        public JwsPartsTests()
        {
            jwsParts = JwsParts.ParseToken(SmartHealthCardFactory.CreateJwsToken());
        }

        [Test]
        public void DecodedHeader_returnsString()
        {
            string decoded = jwsParts.DecodedHeader();

            Assert.NotNull(decoded);
        }

        [Test]
        public async Task DecodedPayload_returnsStringAsync()
        {
            string decoded = await jwsParts.DecodedPayload();

            Assert.NotNull(decoded);
        }


        [Test]
        public void DecodedSignature_returnsByteArray()
        {
            byte[] decoded = jwsParts.DecodedSignature();

            Assert.NotNull(decoded);
        }
    }
}
