using OntologyTypeAheadApi.Context.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Helpers;

namespace OntologyTypeAheadApi.Context.Implementation
{
    public class Elastic_DatastoreContext : IDatastoreContext
    {
        //https://www.elastic.co/guide/en/elasticsearch/reference/current/getting-started.html

        #region private properties
        private Uri _elasticUrl;
        #endregion

        #region public methods
        public async Task<IEnumerable<LookupItem>> All(string accessor)
        {
            return await elasticQuery(accessor, QueryType.All);
        }

        public async Task<IEnumerable<LookupItem>> Contains(string accessor, string query, bool casesensitive = false)
        {
            return await elasticQuery(accessor, QueryType.Contains, query, casesensitive);
        }

        public async Task<IEnumerable<LookupItem>> Equals(string accessor, string query, bool casesensitive = false)
        {
            return await elasticQuery(accessor, QueryType.Exact, query, casesensitive);
        }

        public async Task<IEnumerable<LookupItem>> StartsWith(string accessor, string query, bool casesensitive = false)
        {
            return await elasticQuery(accessor, QueryType.StartsWith, query, casesensitive);
        }

        public async Task Populate(Dictionary<string, string> accessors, Dictionary<string, Dictionary<string, string>> data)
        {
            await trashAllData();
            await addItemsToLookupIndex("accessors", accessors);

            foreach (var x in data)
            {
                await addItemsToLookupIndex(x.Key, x.Value);
            }

        }
        #endregion

        #region constructors
        public Elastic_DatastoreContext(string elasticUrl)
        {
            _elasticUrl = new Uri(elasticUrl);
        }
        #endregion

        #region private methods
        #region query data#
        /// <summary>
        /// Will run the query on elastic
        /// then compensate for Elastic being case insensitive and doing by words
        /// </summary>
        /// <param name="accessor">the document type in the index</param>
        /// <param name="type">Query type</param>
        /// <param name="query">Query string</param>
        /// <param name="casesensitive"></param>
        /// <returns></returns>
        private async Task<IEnumerable<LookupItem>> elasticQuery(string accessor, QueryType type, string query = "", bool casesensitive = false)
        {
            var count = await getCount(accessor);

            if (count > 0)
            {
                var elasticData = await queryElastic(accessor, count, 1, type, query);
                if (type == QueryType.All || elasticData == null || elasticData.Count() == 0)
                    return elasticData;
                if (casesensitive)
                    elasticData = elasticData.Where(x => x.Label.Replace("-"," ").Contains(query.Replace("-", " "))).ToList();
                else
                    elasticData = elasticData.Where(x => x.Label.Replace("-", " ").ToLowerInvariant().Contains(query.Replace("-", " ").ToLowerInvariant())).ToList();
                return elasticData;
            }
            else
                return null;
        }
        #endregion

        #region manage data
        private async Task trashAllData()
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                var r = await client.DeleteAsync("_all");
                
                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true
                //}

                if (r.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Deleting existing Elastic Data did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");
            }
        }
        
        private async Task createElasticIndex(string indexName)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                var r = await client.PutAsync(indexName, new StringContent(""));

                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true,
                //    "shards_acknowledged": true
                //}

                if (r.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Creating Elastic Index {indexName} did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");
            }
        }
        
        private async Task addItemToIndex(string accessor, KeyValuePair<string, string> item)
        {
            var data = new { Label = item.Value };
            var content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None).ToString(), Encoding.UTF8, "application/json");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                var r = await client.PostAsync($"lookupitems/{accessor.ToLowerInvariant()}/{HttpUtility.UrlEncode(item.Key)}", content);

                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true,
                //    "shards_acknowledged": true
                //}

                if (r.StatusCode != HttpStatusCode.Created)
                    throw new Exception($"Creating Elastic item _type = {accessor} Label = {item.Key} did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");

            }
        }

        private static StringBuilder getBulkJsonForItem(string accessor, string id, string label)
        {
            //{ "create":{ "_index":"accessors","_type":"accessors","_id":"a_towns"} }
            //{ "doc" : { "Label":"Towns starting with A"} }
            //{ "create":{ "_index":"a_towns","_type":"a_towns","_id":"http://www.stew.test.uk/dummy_ontology/Abingdon_to_Alston"} }
            //{ "doc" : { "Label":"Abingdon to Alston"} }

            var data1 = new { create = new { _index = accessor.ToLowerInvariant(), _type = accessor.ToLowerInvariant(), _id = id } };
            var data2 = new { doc = new { Label = label } };

            var json1 = JsonConvert.SerializeObject(data1, Formatting.None);
            var json2 = JsonConvert.SerializeObject(data2, Formatting.None);

            var ret = new StringBuilder(json1).Append(Environment.NewLine).Append(json2).Append(Environment.NewLine);

            return ret;
        }

        private async Task addItemsToLookupIndex(string accessor, Dictionary<string, string> items)
        {
            if (items != null && items.Count > 0)
            {
                var datasb = getBulkJsonForItem(accessor, items.ToList()[0].Key, items.ToList()[0].Value);
                items.ToList().Skip(1).ToList().ForEach(x => datasb.Append(getBulkJsonForItem(accessor, x.Key, x.Value)));

                var xx = datasb.ToString();
                var content = new StringContent(datasb.ToString(), Encoding.UTF8, "application/json");
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    client.BaseAddress = _elasticUrl;
                    var r = await client.PostAsync("_bulk", content);

                    //Success equals:
                    //HTTP200
                    //{
                    //  "took": 409,
                    //  "errors": false,
                    //  "items": [
                    //    {
                    //      ...

                    if (r.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Creating Elastic items for _type = {accessor} did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");

                }
            } 
        }

        private async Task<int> getCount(string accessor)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                var r = await client.GetAsync($"{accessor}/_count");

                dynamic resp = await HttpResponseHelper.ReadResponseContent(r.Content);

                //Success equals:
                //HTTP200
                //{
                //    "count": 42,
                //    "_shards":  {
                //        "total": 5,
                //        "successful": 5,
                //        "failed": 0
                //    }
                //}

                if (r.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Creating Elastic item count for _type = {accessor} did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");

                return resp.count;
            }
        }

        private async Task<IEnumerable<LookupItem>> queryElastic(string accessor, int pageSize, int page, QueryType type, string query, bool caseSensitive = false)
        {
            var queryJson = ElasticHelper.GetQueryJson(pageSize, page, type, query);

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                var content = new StringContent(queryJson, Encoding.UTF8, "application/json");
                var r = await client.PostAsync($"{accessor}/_search", content);

                //Success equals:
                //HTTP200
                //{
                //    "took":3,
                //    "timed_out":false,
                //    "_shards":{
                //        "total":5,
                //        "successful":5,
                //        "skipped":0,
                //        "failed":0
                //    },
                //    "hits":{
                //        "total":1,
                //        "max_score":1.0,
                //        "hits":[
                //            {
                //                "_index":"accessors",
                //                "_type":"accessors",
                //                "_id":"a_towns",
                //                "_score":1.0,
                //                "_source":{
                //                    "doc":{
                //                        "Label":"Towns starting with A"
                //                    }
                //                }
                //            }
                //        ]
                //    }
                //}

                if (r.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Querying Elastic on {accessor}/_count with {queryJson} did not return OK - {r.StatusCode.ToString()} - {r.Content.ToString()}");

                dynamic resp = await HttpResponseHelper.ReadResponseContent(r.Content);
                List<LookupItem> ret = new List<LookupItem>();
                if (resp.hits != null)
                {
                    ((IEnumerable<dynamic>)resp.hits.hits).ToList().ForEach(x =>
                    {
                        ret.Add(new LookupItem(x._id.ToString(), x._source.doc.Label.ToString()));
                    });
                }

                return ret.Count > 0 ? ret : null;
            }
        }

        #endregion
        #endregion
    }
}
