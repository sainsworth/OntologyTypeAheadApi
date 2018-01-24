using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Elastic
{
    public class ElasticLookupItem
    {
        public string Label { get; set; }
        public ElasticLookupItem() { }
        public ElasticLookupItem(string label)
        {
            Label = label;
        }
    }
}