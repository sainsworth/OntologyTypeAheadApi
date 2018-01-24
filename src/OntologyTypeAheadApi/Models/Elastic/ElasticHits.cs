using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticHits<T>
    {
        public int total { get; set; }
        public float max_score { get; set; }
        public IEnumerable<ElasticHitItem<T>> hits { get; set; }
    }
}
