using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class LookupResponse
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public LookupResponse(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}