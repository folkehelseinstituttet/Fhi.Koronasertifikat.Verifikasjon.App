using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FHICORC.Tests.ModelTests
{
    public class SmartHealthCardPersonNameTests
    {
        [TestCase("Anyperson, John B.", "{\"family\":\"Anyperson\",\"given\":[\"John\",\"B.\"]}")]
        [TestCase("Anyperson, John", "{\"family\":\"Anyperson\",\"given\":[\"John\"]}")]
        [TestCase("Anyperson", "{\"family\":\"Anyperson\",\"given\":[]}")]
        [TestCase("Anyperson", "{\"family\":\"Anyperson\"}")]
        public void FullName_HandlesDifferentData(string expected, string json)
        {
            SmartHealthCardPersonName personName = JsonConvert.DeserializeObject<SmartHealthCardPersonName>(json);

            Assert.AreEqual(expected, personName.FullName);
        }
    }
}
