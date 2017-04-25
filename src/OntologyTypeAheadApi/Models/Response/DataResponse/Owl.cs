using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class Owl
    {
        public string Label { get; set; }
        public string Uri { get; set; }
        public List<Owl> Children { get; set; }

        public Owl()
        {
            Children = new List<Owl>();
        }
    }
}