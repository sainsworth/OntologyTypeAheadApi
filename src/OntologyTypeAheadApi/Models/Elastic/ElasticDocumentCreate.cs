using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticDocumentCreate
    {
        public ElasticDocumentHeader create { get; set; } = new ElasticDocumentHeader();

        public ElasticDocumentCreate() { }
        public ElasticDocumentCreate(string index, string type, string id)
        {
            create = new ElasticDocumentHeader(index, type, id);
        }
    }
}