using OntologyTypeAheadApi.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OntologyTypeAheadApi.Models.Response
{
    public class EmptyResponse : IResponse
    {
        public Exception Exception { get; set; }

        public string Message { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        public EmptyResponse()
        {
            Status = ResponseStatus.NotSet;
        }
    }
}