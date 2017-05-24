using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using OntologyTypeAheadApi.Helpers;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Service.Contract;
using OntologyTypeAheadApi.Models.Request;
using System.Threading.Tasks;
using OntologyTypeAheadApi.Models.Response;

namespace OntologyTypeAheadApi.Controllers
{
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("admin/ping")]
        public async Task<HttpResponseMessage> Ping()
        {
            var request = new ConformedRequest()
            {
                Route = "admin/ping"
            };

            return HttpResponseHelper.StandardiseResponse(request, new EmptyResponse() { Status = Enums.ResponseStatus.OK, Message = "I'm here!!"} );
        }
    }
}
        