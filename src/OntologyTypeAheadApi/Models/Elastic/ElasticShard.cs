using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticShard
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
        public int skipped { get; set; }

        public ElasticShard() {}
    }
}