using OntologyTypeAheadApi.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.Envelope
{
    public class ResponseEnvelope
    {
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }

        public ResponseEnvelope(IRequest request, IResponse response)
        {
            Request = request;
            Response = response;
        }
    }

    //public class ResponseEnvelope<T>
    //{
    //    public IRequest Request { get; set; }
    //    public IResponse<T> Response { get; set; }

    //    public ResponseEnvelope(IRequest request, IResponse<T> response)
    //    {
    //        Request = request;
    //        Response = response;
    //    }
    //}
}