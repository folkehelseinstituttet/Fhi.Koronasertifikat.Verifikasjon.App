using FHICORC.Utils;
using NUnit.Framework;
using Xamarin.Forms;

namespace FHICORC.Tests.UITests
{
    public class FHICORCColorUITests : BaseUITests
    {
        [TestCase(FHICORCColor.TitleTextColor, "#1c1999")]
        [TestCase(FHICORCColor.LightTextColor, "#77819A")]
        [TestCase(FHICORCColor.ContentTextColor, "#47526F")]
        [TestCase(FHICORCColor.BaseTextColor, "#24215f")]
        public void TestValidFullName(FHICORCColor actualColor, string resourceValue)
        {
            //when calling .Color on the enum
            Color actual = actualColor.Color();
            //then it will fetch the right color from the resource dict
            Assert.AreEqual(Color.FromHex(resourceValue), actual);
        }
    }
}