using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticSource<T>
    {
        public T doc { get; set; }
        public ElasticSource() { }
        public ElasticSource(T document)
        {
            doc = document;
        }
    }
}