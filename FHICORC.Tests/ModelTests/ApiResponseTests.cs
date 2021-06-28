using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;

namespace FHICORC.Tests.Models
{
    public class ApiResponseTests
    {
        public ApiResponseTests()
        {
        }

        [Test]
        public void ResponseText_SetValueAndGetValue_ShouldReturnCorrectValue()
        {
            string responseTxt = "response text1";
            ApiResponse res = new ApiResponse("url");
            res.ResponseText = responseTxt;
            Assert.AreEqual(responseTxt, res.ResponseText);
        }

        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public void StatusCode_SetValueAndGetValue_ShouldReturnCorrectValue(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("url");
            res.StatusCode = (int) httpStatus;
            Assert.AreEqual((int) httpStatus, res.StatusCode);
        }

        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.Created)]
        public void isSuccessful_HaveStatusCreatedOrOK_ShouldReturnTrue(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("url");
            res.StatusCode = (int) httpStatus;
            Assert.True(res.IsSuccessfull);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.BadGateway)]
        public void isSuccessful_HaveStatusNotEqualCreatedAndOK_ShouldReturnFalse(HttpStatusCode httpStatus)
        {
            ApiResponse res = new ApiResponse("Url");
            res.StatusCode = (int) httpStatus;
            Assert.False(res.IsSuccessfull);
        }

        [TestCase("publickeys")]
        [TestCase("passports")]
        public void ApiResponse_CalculateEndpoint(string endpoint)
        {
            string completeUrl = $"https://myserver.dk/api/v1/{endpoint}";
            ApiResponse res = new ApiResponse(completeUrl);

            Assert.AreEqual(endpoint, res.Endpoint);
        }
    }
}
