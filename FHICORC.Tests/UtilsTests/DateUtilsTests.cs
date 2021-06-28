using System;
using FHICORC.Utils;
using NUnit.Framework;

namespace FHICORC.Tests.UtilsTests
{
    public class DateUtilsTests
    {
        [TestCase("01/08/2021 14:00:00", "08. jan. 2021")]
        [TestCase("05/29/2021", "29. may. 2021")]
        [TestCase("1/9/2021 05:50:06", "09. jan. 2021")]
        [TestCase("2021/10/12", "12. oct. 2021")]
        [TestCase("2021-02-28T05:50:06", "28. feb. 2021")]
        public void TestFormatDateToScannerResultFormat(DateTime date, string expected)
        {
            var actual = date.DateToScannerResultFormat();
            
            Assert.AreEqual(expected, actual);
        }
    }
}