using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace OntologyTypeAheadApi.Models.Request
{
    public class ElasticQuery
    {
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public Elastic_OuterQuery Query { get; set; }
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public int From { get; set; }
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public int Size { get; set; }
    }
    public class Elastic_OuterQuery
    {
        [JsonProperty("bool", NullValueHandling = NullValueHandling.Ignore)]
        public Elastic_Bool Bool { get; set; }
        [JsonProperty("match_all", NullValueHandling = NullValueHandling.Ignore)]
        public Elastic_MatchAll MatchAll { get; set; }
    }

    public class Elastic_Bool
    {

        [JsonProperty("must", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Elastic_QueryStringOuter> Must { get; set; }
    }

    public class Elastic_QueryStringOuter
    {
        [JsonProperty("query_string", NullValueHandling = NullValueHandling.Ignore)]
        public Elastic_QueryString QueryString { get; set; }
    }

    public class Elastic_QueryString
    {
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public string Query { get; set; }
    }

    public class Elastic_MatchAll
    {

    }
}