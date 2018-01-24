using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticCountResponse
    {
        public int count { get; set; }
        public ElasticShard _shards { get; set; }
    }
}