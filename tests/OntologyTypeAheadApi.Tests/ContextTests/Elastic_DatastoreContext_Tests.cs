using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Context;
using OntologyTypeAheadApi.Context.Implementation;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Context.Contract;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Tests.ContextTests
{
    [TestClass]
    class Elastic_DatastoreContext_Tests
    {
        //public static IDatastoreContext elastic_context = new Elastic_DatastoreContext("http://fake.elastic.url/");
        // This needs integration tests instead

        //[TestMethod]
        //public async Task Mock_DatastoreContext_All()
        //{
        //    var ret = await elastic_context.All("test");
        //    Assert.AreEqual(6, ret.Count());
        //}
    }
}