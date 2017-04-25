using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Context;
using OntologyTypeAheadApi.Context.Implementation;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Context.Contract;

namespace OntologyTypeAheadApi.Tests.Context
{
    [TestClass]
    public class DatastoreContextTests
    {
        public static IEnumerable<LookupResponse> data = new List<LookupResponse>() {
            new LookupResponse("1", "One"),
            new LookupResponse("2", "Two"),
            new LookupResponse("3", "Three"),
            new LookupResponse("4", "Four"),
            new LookupResponse("40", "FOURTY"),
            new LookupResponse("41", "Fourty one"),
        };

        public static IDatastoreContext context = new Mock_DatastoreContext(data);

        [TestMethod]
        public void Mock_LookupContext_All()
        {
            var ret = context.All();
            Assert.AreEqual(6, ret.Count());
        }

        [TestMethod]
        public void Mock_LookupContext_Equals()
        {
            var ret = context.Equals("ONE");
            Assert.AreEqual(1, ret.Count());
            Assert.AreEqual("One", ret.First().Label);
        }

        [TestMethod]
        public void Mock_LookupContext_Equals_CaseSensitive()
        {
            var ret_cs = context.Equals("ONE", true);
            Assert.AreEqual(null, ret_cs.FirstOrDefault());
        }

        [TestMethod]
        public void Mock_LookupContext_StartsWith()
        {
            var ret = context.StartsWith("FOUR").ToList();
            Assert.AreEqual(3, ret.Count());
            Assert.AreEqual("Four", ret.First().Label);
        }

        [TestMethod]
        public void Mock_LookupContext_StartsWith_CaseSensitive()
        {
            var ret_cs = context.StartsWith("FOUR", true);
            Assert.AreEqual(1, ret_cs.Count());
            Assert.AreEqual("FOURTY", ret_cs.First().Label);
        }

        [TestMethod]
        public void Mock_LookupContext_Contains()
        {
            var ret = context.Contains("t").ToList();
            Assert.AreEqual(4, ret.Count());
            Assert.AreEqual("Two", ret.First().Label);
        }

        [TestMethod]
        public void Mock_LookupContext_Contains_CaseSensitive()
        {
            var ret_cs = context.Contains("t", true);
            Assert.AreEqual(1, ret_cs.Count());
            Assert.AreEqual("Fourty one", ret_cs.First().Label);
        }
    }
}
