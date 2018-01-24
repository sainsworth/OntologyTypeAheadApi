using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticSearchResponse<T>
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public ElasticShard _shards { get; set; }
        public ElasticHits<T> hits { get; set; }
    }
}