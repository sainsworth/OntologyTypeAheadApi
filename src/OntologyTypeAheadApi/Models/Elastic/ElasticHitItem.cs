using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticHitItem<T>
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public float _score { get; set; }
        public ElasticSource<T> _source { get; set; } = new ElasticSource<T>();

        public ElasticHitItem() { }
    }
}