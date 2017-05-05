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
        public IEnumerable<Ontology> All()
        {
            return new List<Ontology>() {
                new Ontology(
                    Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,@"App_Data\Dummy_Ontology.ttl"),
                    "dummy",
                    new List<string>() { "http://www.stew.test.uk/dummy_ontology/Starts_with_A"},
                    RdfSource.File,
                    RdfType.TTL)
            };
        }
    }
}