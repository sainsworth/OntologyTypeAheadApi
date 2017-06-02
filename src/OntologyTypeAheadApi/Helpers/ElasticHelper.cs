using Newtonsoft.Json;
using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Extensions;
using OntologyTypeAheadApi.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;

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