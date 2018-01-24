using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Models.Request;
using OntologyTypeAheadApi.Models.Response;
using OntologyTypeAheadApi.Helpers;

namespace OntologyTypeAheadApi.Tests.Helpers
{
    [TestClass]
    public class HttpResponseHelperTests
    {

        public ConformedRequest request = new ConformedRequest()
        {
            Route = "test route",
            Details = "test details"
        };

        [TestMethod]
        public void HttpResponseHelper_StandardiseResponse_OK()
        {
           
            var response = new EnumerableResponse<string>() {
                Status = Enums.ResponseStatus.OK,
                Data = new List<string> { "test data" }
            };

            var result = HttpResponseHelper.StandardiseResponse(request, response);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(@"{""Request"":{""Route"":""test route"",""Details"":""test details""},""Response"":{""Status"":""OK"",""Data"":[""test data""]}}", result.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void HttpResponseHelper_StandardiseResponse_NoResponse()
        {

            var response = new EnumerableResponse<string>()
            {
                Status = Enums.ResponseStatus.NoResponse
            };

            var result = HttpResponseHelper.StandardiseResponse(request, response);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(@"{""Request"":{""Route"":""test route"",""Details"":""test details""},""Response"":{""Status"":""NoResponse""}}", result.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void HttpResponseHelper_StandardiseResponse_InvalidRequest()
        {

            var response = new EnumerableResponse<string>()
            {
                Status = Enums.ResponseStatus.InvalidRequest,
                Message = "invalid"
            };

            var result = HttpResponseHelper.StandardiseResponse(request, response);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(@"{""Request"":{""Route"":""test route"",""Details"":""test details""},""Response"":{""Status"":""InvalidRequest"",""Message"":""invalid""}}", result.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public void HttpResponseHelper_StandardiseResponse_Error()
        {

            var response = new EnumerableResponse<string>()
            {
                Status = Enums.ResponseStatus.Error,
                Exception = new System.Exception("outer", new System.Exception("inner"))
            };

            var result = HttpResponseHelper.StandardiseResponse(request, response);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(@"{""Request"":{""Route"":""test route"",""Details"":""test details""},""Response"":{""Status"":""Error"",""Exception"":""inner""}}", result.Content.ReadAsStringAsync().Result);
        }
    }
}
