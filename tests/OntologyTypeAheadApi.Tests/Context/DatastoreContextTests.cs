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
        // to establish the behaviour expected of the interface 
        // andprovide confidence for tests using the mock instance
        #region Mock_DatastoreContext

        public static IEnumerable<LookupItem> data = new List<LookupItem>() {
            new LookupItem("1", "One"),
            new LookupItem("2", "Two"),
            new LookupItem("3", "Three"),
            new LookupItem("4", "Four"),
            new LookupItem("40", "FOURTY"),
            new LookupItem("41", "Fourty one"),
        };
        public static Dictionary<string, IEnumerable<LookupItem>> datastore = new Dictionary<string, IEnumerable<LookupItem>>
        {
            { "test", data }
        };

        public static IDatastoreContext mock_context = new Mock_DatastoreContext(datastore);

        [TestMethod]
        public void Mock_DatastoreContext_All()
        {
            var ret = mock_context.All("test");
            Assert.AreEqual(6, ret.Count());
        }

        [TestMethod]
        public void Mock_DatastoreContext_Equals()
        {
            var ret = mock_context.Equals("test","ONE");
            Assert.AreEqual(1, ret.Count());
            Assert.AreEqual("One", ret.First().Label);
        }

        [TestMethod]
        public void Mock_DatastoreContext_Equals_CaseSensitive()
        {
            var ret_cs = mock_context.Equals("test","ONE", true);
            Assert.AreEqual(null, ret_cs.FirstOrDefault());
        }

        [TestMethod]
        public void Mock_DatastoreContext_StartsWith()
        {
            var ret = mock_context.StartsWith("test","FOUR").ToList();
            Assert.AreEqual(3, ret.Count());
            Assert.AreEqual("Four", ret.First().Label);
        }

        [TestMethod]
        public void Mock_DatastoreContext_StartsWith_CaseSensitive()
        {
            var ret_cs = mock_context.StartsWith("test","FOUR", true);
            Assert.AreEqual(1, ret_cs.Count());
            Assert.AreEqual("FOURTY", ret_cs.First().Label);
        }

        [TestMethod]
        public void Mock_DatastoreContext_Contains()
        {
            var ret = mock_context.Contains("test","t").ToList();
            Assert.AreEqual(4, ret.Count());
            Assert.AreEqual("Two", ret.First().Label);
        }

        [TestMethod]
        public void Mock_DatastoreContext_Contains_CaseSensitive()
        {
            var ret_cs = mock_context.Contains("test","t", true);
            Assert.AreEqual(1, ret_cs.Count());
            Assert.AreEqual("Fourty one", ret_cs.First().Label);
        }

        #endregion
    }
}
