using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OntologyTypeAheadApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class LoadOntologyResponse
    {
        public string Location { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }
        public Exception Exception { get; set; }

        public LoadOntologyResponse(string location, ResponseStatus status = ResponseStatus.DataLoaded, Exception e = null)
        {
            Location = location;
            Status = status;
            Exception = e;
        }
    }
}