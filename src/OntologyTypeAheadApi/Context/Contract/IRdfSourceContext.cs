using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Collections.Generic;

namespace OntologyTypeAheadApi.Context.Contract
{
    public interface IRdfSourceContext
    {
        IEnumerable<RdfResponse> All();
    }
}
