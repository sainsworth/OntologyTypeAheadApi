using Newtonsoft.Json;
using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Extensions;
using OntologyTypeAheadApi.Models.Elastic;
using OntologyTypeAheadApi.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OntologyTypeAheadApi.Helpers
{
    public static class ElasticHelper
    {
        public static string GetQueryJson (int pageSize, int page, QueryType type, string query)
        {
            if (string.IsNullOrWhiteSpace(query) && type != QueryType.All)
                return null;

            var elasticQuery = new ElasticQuery()
            {
                From = pageSize * (page - 1),
                Size = pageSize,
                Query = new Elastic_OuterQuery()
            };

            if (type == QueryType.All)
                elasticQuery.Query.MatchAll = new Elastic_MatchAll();
            else
            {
                var start = type == QueryType.Contains ? "*" : "";
                var end = type != QueryType.All ? "*" : "";
                string q = queryStringFromquery(query, start, end);

                elasticQuery.Query.Bool = new Elastic_Bool()
                {
                    Must = new List<Elastic_QueryStringOuter>() {
                        new Elastic_QueryStringOuter() {
                            QueryString = new Elastic_QueryString() {
                                Query = q
                            }
                        }
                    }
                };

            }

            return JsonConvert.SerializeObject(elasticQuery, Formatting.None);
        }

        public static string GetBulkJsonForItem(string accessor, string id, string label)
        {
            //{ "create":{ "_index":"accessors","_type":"accessors","_id":"a_towns"} }
            //{ "doc" : { "Label":"Towns starting with A"} }
            //{ "create":{ "_index":"a_towns","_type":"a_towns","_id":"http://www.stew.test.uk/dummy_ontology/Abingdon_to_Alston"} }
            //{ "doc" : { "Label":"Abingdon to Alston"} }

            var data1 = new ElasticDocumentCreate(accessor, accessor, id);
            var data2 = new ElasticSource<ElasticLookupItem>(new ElasticLookupItem(label));

            var json1 = JsonConvert.SerializeObject(data1, Formatting.None);
            var json2 = JsonConvert.SerializeObject(data2, Formatting.None);

            var ret = new StringBuilder(json1).Append(Environment.NewLine).Append(json2).Append(Environment.NewLine);

            return ret.ToString();
        }

        public static string GetBulkJsonForItems(string accessor, Dictionary<string, string> items)
        {
            return string.Join("", items.Select(x => GetBulkJsonForItem(accessor, x.Key, x.Value)));
        }

        private static string queryStringFromquery(string query, string start, string end)
        {
            var words = query
                            .Replace("-"," ")
                            .ElasticEscaped()
                            .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var queryString = words[0];
            words.Skip(1).ToList().ForEach(x => {
                queryString = $"{queryString} AND {x}";
            });

            return $"{start}{queryString}{end}";
        }
    }
}