using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class LookupItem
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public LookupItem(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}