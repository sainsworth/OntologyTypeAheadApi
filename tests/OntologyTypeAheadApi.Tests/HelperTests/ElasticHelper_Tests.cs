using Newtonsoft.Json;
using OntologyTypeAheadApi.Models.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Helpers;
using Shouldly;

namespace OntologyTypeAheadApi.Tests.HelperTests
{
    [TestClass]
    public class  ElasticHelper_Tests
    {
        private static Dictionary<string, string> test_items = new Dictionary<string, string>()
        {
            { "One", "Item One" },
            { "Two", "Item two" }
        };

        private static string accessor = "Accessor";

        [TestMethod]
        public void ElasticHelper_GetBulkJsonForItem()
        {
            var result = ElasticHelper.GetBulkJsonForItem(accessor, "One", "Item One");

            result.ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }

        [TestMethod]
        public void ElasticHelper_GetBulkJsonForItems()
        {
            var result = ElasticHelper.GetBulkJsonForItems(accessor, test_items);

            result.ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }

        [TestMethod]
        public void ElasticHelper_GetQueryJson_All()
        {
            ElasticHelper.GetQueryJson(10, 2, Enums.QueryType.All, "").ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }

        [TestMethod]
        public void ElasticHelper_GetQueryJson_Contains()
        {
            ElasticHelper.GetQueryJson(10, 2, Enums.QueryType.Contains, "Contains").ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }

        [TestMethod]
        public void ElasticHelper_GetQueryJson_StartsWith()
        {
            ElasticHelper.GetQueryJson(10, 2, Enums.QueryType.StartsWith, "StartsWith").ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }

        [TestMethod]
        public void ElasticHelper_GetQueryJson_Exact()
        {
            ElasticHelper.GetQueryJson(10, 2, Enums.QueryType.Exact, "Exact").ShouldMatchApproved(x =>
            {
                x.SubFolder("Approvals");
            });
        }
    }
}
