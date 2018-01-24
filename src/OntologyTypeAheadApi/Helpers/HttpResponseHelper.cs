using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using OntologyTypeAheadApi.Models.Response.Envelope;
using System.Threading.Tasks;
using System.IO;

namespace OntologyTypeAheadApi.Helpers
{

    public static class HttpResponseHelper
    {
        public static HttpResponseMessage StandardiseResponse(IRequest request, IResponse response)
        {
            var resp = new ResponseEnvelope(request, response);
            var ret = new HttpResponseMessage(HttpStatusCode.OK);
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            ret.Content = new StringContent(JsonConvert.SerializeObject(resp, Formatting.None, settings).ToString(), Encoding.UTF8, "application/json");
            switch (response.Status)
            {
                case ResponseStatus.OK:
                case ResponseStatus.NoResponse:
                case ResponseStatus.DataLoaded:
                    return ret;
                case ResponseStatus.InvalidRequest:
                default:
                    ret.StatusCode = HttpStatusCode.BadRequest;
                    return ret;
            }
        }

        public static async Task<T> ReadResponseContent<T>(HttpContent content)
        {
            Stream stream = await content.ReadAsStreamAsync();
            StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
            string text = readStream.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}