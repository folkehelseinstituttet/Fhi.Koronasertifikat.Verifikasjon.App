using System;
using FHICORC.Core.Services.Utils;
using JsonLogic.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace FHICORC.Tests.UtilsTests
{
    public class JsonLogicUtilsTests
    {

        private JsonLogicEvaluator evaluator;

        private Func<string, string, string, string> binaryCompareExp = (op, s1, s2) => $"{{\"{op}\": [\"{s1}\", \"{s2}\"]}}";

        private Func<string, string, string, string, string> ternaryCompareExp = (op, s1, s2, s3) => $"{{\"{op}\": [\"{s1}\", \"{s2}\", \"{s3}\"]}}";

        [OneTimeSetUp]
        public void SetUp()
        {
            EvaluateOperators operators = JsonLogicUtils.GetStringSupportedOperators();
            evaluator = new JsonLogicEvaluator(operators);
        }

        [TestCase("<", "2020-06-05", "2021-06-05", true)]
        [TestCase("<","2020-06-05", "2020-04-05", false)]
        [TestCase("<=", "2020-06-05", "2020-06-05", true)]
        [TestCase("<=", "2020-06-05", "2020-04-05", false)]
        [TestCase(">", "2020-06-05", "2020-06-05", false)]
        [TestCase(">", "2020-06-05", "2020-04-05", true)]
        [TestCase(">=", "2020-06-05", "2020-06-05", true)]
        [TestCase(">=", "2020-06-05", "2020-12-05", false)]
        public void TestBinaryLexicographicalCompare(string op, string s1, string s2, bool expected)
        {
            var jsonText = binaryCompareExp(op, s1, s2);
            var rule = JObject.Parse(jsonText);
            object actual = evaluator.Apply(rule, null);
            
            Assert.AreEqual(expected, actual);
        }

        [TestCase("<", "2020-06-05", "2021-06-05", "2022-01-01", true)]
        [TestCase("<", "2020-06-05", "2023-06-05", "2022-01-01", false)]
        [TestCase("<=", "2020-06-05", "2020-06-05", "2022-01-01", true)]
        [TestCase("<=", "2021-06-05", "2020-06-05", "2022-01-01", false)]
        [TestCase(">", "2020-06-05", "2019-06-05", "2018-01-01", true)]
        [TestCase(">", "2020-06-05", "2021-06-05", "2022-01-01", false)]
        [TestCase(">=", "2020-06-05", "2020-06-05", "2020-06-05", true)]
        [TestCase(">=", "2020-06-05", "2021-06-05", "2022-01-01", false)]
        public void TestTernaryLexicographicalCompare(string op, string s1, string s2, string s3, bool expected)
        {
            var jsonText = ternaryCompareExp(op, s1, s2, s3);
            var rule = JObject.Parse(jsonText);
            object actual = evaluator.Apply(rule, null);

            Assert.AreEqual(expected, actual);
        }
    }
}
