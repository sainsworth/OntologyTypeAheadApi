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

namespace OntologyTypeAheadApi.Context.Implementation
{
    public class Elastic_DatastoreContext : IDatastoreContext
    {
        //https://www.elastic.co/guide/en/elasticsearch/reference/current/getting-started.html

        private Uri _elasticUrl;

        public IEnumerable<LookupItem> All(string accessor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LookupItem> Contains(string accessor, string query, bool casesensitive = false)
        {
            var count = getCount(accessor);

            //var responseData = 

            throw new NotImplementedException();
        }

        public IEnumerable<LookupItem> Equals(string accessor, string query, bool casesensitive = false)
        {
            throw new NotImplementedException();
        }

        public void Populate(Dictionary<string, Dictionary<string, string>> data)
        {
            trashAllData();

            foreach(var x in data)
            {
                addItemsToIndex(x.Key, x.Value);
            }

        }

        public IEnumerable<LookupItem> StartsWith(string accessor, string query, bool casesensitive = false)
        {
            throw new NotImplementedException();
        }

        public Elastic_DatastoreContext(string elasticUrl)
        {
            _elasticUrl = new Uri(elasticUrl);
        }

        private void trashAllData()
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                Task<HttpResponseMessage> response = Task.Run(async () =>
                {
                    var r = await client.DeleteAsync("_all");
                    return r;
                });

                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true
                //}

                if (response.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Deleting existing Elastic Data did not return OK - {response.Result.StatusCode.ToString()} - {response.Result.Content.ToString()}");
            }
        }
        
        private void createElasticIndex(string indexName)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                Task<HttpResponseMessage> response = Task.Run(async () =>
                {
                    var r = await client.PutAsync(indexName, new StringContent(""));
                    return r;
                });

                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true,
                //    "shards_acknowledged": true
                //}

                if (response.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Creating Elastic Index {indexName} did not return OK - {response.Result.StatusCode.ToString()} - {response.Result.Content.ToString()}");
            }
        }

        // uses our Id as the elastic Id
        private void addItemToIndex(string accessor, KeyValuePair<string, string> item)
        {
            var data = new { Label = item.Value };
            var content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None).ToString(), Encoding.UTF8, "application/json");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = _elasticUrl;
                Task<HttpResponseMessage> response = Task.Run(async () =>
                {
                    var r = await client.PostAsync($"lookupitems/{accessor.ToLowerInvariant()}/{HttpUtility.UrlEncode(item.Key)}", content);
                    return r;
                });

                //Success equals:
                //HTTP200
                //{
                //    "acknowledged": true,
                //    "shards_acknowledged": true
                //}

                if (response.Result.StatusCode != HttpStatusCode.Created)
                    throw new Exception($"Creating Elastic item _type = {accessor} Label = {item.Key} did not return OK - {response.Result.StatusCode.ToString()} - {response.Result.Content.ToString()}");

            }
        }

        private StringBuilder getBulkJsonForItem(string accessor, string id, string label)
        {
            //{ "create": { "_index": "website", "_type": "blog", "_id": "123" } }
            //{ "title":    "My first blog post" }

            var data1 = new { create = new { _index = "lookupitems", _type = accessor.ToLowerInvariant(), _id = id } };
            var data2 = new { Label = label };

            var json1 = JsonConvert.SerializeObject(data1, Formatting.None);
            var json2 = JsonConvert.SerializeObject(data2, Formatting.None);

            var ret = new StringBuilder(json1).Append(Environment.NewLine).Append(json2);

            return ret;
        }

        private void addItemsToIndex(string accessor, Dictionary<string, string> items)
        {
            if (items != null && items.Count > 0)
            {
                var datasb = getBulkJsonForItem(accessor, items.ToList()[0].Key, items.ToList()[0].Value);
                items.ToList().Skip(1).ToList().ForEach(x => datasb.Append(Environment.NewLine).Append(getBulkJsonForItem(accessor, x.Key, x.Value)));

                var xx = datasb.ToString();
                var content = new StringContent(datasb.ToString(), Encoding.UTF8, "application/json");
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
                {
                    client.BaseAddress = _elasticUrl;
                    Task<HttpResponseMessage> response = Task.Run(async () =>
                    {
                        var r = await client.PostAsync("_bulk", content);
                        return r;
                    });

                    //Success equals:
                    //HTTP200
                    //{
                    //  "took": 409,
                    //  "errors": false,
                    //  "items": [
                    //    {
                    //      ...

                    if (response.Result.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Creating Elastic items for _type = {accessor} did not return OK - {response.Result.StatusCode.ToString()} - {response.Result.Content.ToString()}");

                }
            } 
        }

        private int getCount(string accessor)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                int count = 0;
                dynamic resp = null;

                client.BaseAddress = _elasticUrl;
                Task<HttpResponseMessage> response = Task.Run(async () =>
                {
                    var r = await client.GetAsync($"lookupitems/{accessor}/_count");

                    Stream stream = await r.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string text = readStream.ReadToEnd();

                    resp = JsonConvert.DeserializeObject(text);

                    return r;
                });

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

                if (response.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Creating Elastic items for _type = {accessor} did not return OK - {response.Result.StatusCode.ToString()} - {response.Result.Content.ToString()}");

                count = resp.count;

                return count;
            }
        }
    }
}