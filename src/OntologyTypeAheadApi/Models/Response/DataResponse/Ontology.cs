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
        public RdfSource RdfSource { get; set; } = RdfSource.Unknown;
        public RdfType RdfType { get; set; } = RdfType.Unknown;
        public string Accessor { get; set; }
        public string Label { get; set; }
        public IEnumerable<string> RootTypes { get; set; }

        public Ontology(string location, string accessor, string label, IEnumerable<string> rootTypes, RdfSource source, RdfType type = RdfType.Unknown)
        {
            Location = location;
            Accessor = accessor;
            Label = label;
            RdfSource = source;
            RdfType = type;
            RootTypes = rootTypes;
        }
    }
}