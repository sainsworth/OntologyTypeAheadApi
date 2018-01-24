using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticDocumentHeader
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }

        public ElasticDocumentHeader() { }

        public ElasticDocumentHeader(string index, string type, string id)
        {
            _index = index.ToLowerInvariant();
            _type = type.ToLowerInvariant();
            _id = id;
        }
    }
}