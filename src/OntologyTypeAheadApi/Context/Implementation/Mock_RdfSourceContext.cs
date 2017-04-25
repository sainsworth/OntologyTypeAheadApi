using OntologyTypeAheadApi.Context.Contract;
using OntologyTypeAheadApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.IO;

namespace OntologyTypeAheadApi.Context.Implementation
{
    public class Mock_RdfSourceContext : IRdfSourceContext
    {
        public IEnumerable<RdfResponse> All()
        {
            return new List<RdfResponse>() {
                new RdfResponse(
                    Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,@"App_Data\Dummy_Ontology.ttl"),
                    RdfSource.File,
                    RdfType.TTL)
            };
        }
    }
}