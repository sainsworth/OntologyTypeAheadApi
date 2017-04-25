using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Enums;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class Ontology
    {
        public string Location { get; set; }
        public RdfSource Source { get; set; } = RdfSource.Unknown;
        public RdfType Type { get; set; } = RdfType.Unknown;

        public Ontology(string location, RdfSource source, RdfType type = RdfType.Unknown)
        {
            Location = location;
            Source = source;
            Type = type;
        }
    }
}