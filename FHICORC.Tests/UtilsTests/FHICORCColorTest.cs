using System;
using FHICORC.Configuration;
using FHICORC.Utils;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;
using FHICORC.Services.Interfaces;
using FHICORC.Services.Mocks;
using FHICORC.Tests.TestMocks;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;

namespace FHICORC.Tests.UtilsTests
{
    public class FHICORCColorTest : BaseUITests
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