using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using OntologyTypeAheadApi.Helpers;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Service.Contract;
using OntologyTypeAheadApi.Models.Request;

namespace OntologyTypeAheadApi.Controllers
{
    public class LookupController : ApiController
    {
        private readonly IDatastoreService _datastoreService;

        [HttpGet]
        [Route("lookup/{accessor}/")]
        public HttpResponseMessage LookupFromQuery(string accessor, string query)
        {
            var request = new ConformedRequest()
            {
                Route = "lookup/{accessor}",
                Details = new { Accessor = accessor, Query = query }
            };
            var response = _datastoreService.QueryDatastore_Contains(accessor, query);

            return HttpResponseHelper.StandardiseResponse(request, response);
        }

        public LookupController(IDatastoreService datastoreService)
        {
            _datastoreService = datastoreService;
        }
    }
}