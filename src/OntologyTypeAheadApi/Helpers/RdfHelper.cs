using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace OntologyTypeAheadApi.Helpers
{

    public static class RdfHelper
    {
        public static IGraph GetGraphFromOntology(Ontology ontology)
        {
            IGraph g = new Graph();
            IRdfReader r = ontology.Type == RdfType.TTL ? new TurtleParser() : null;

            if (ontology.Source == RdfSource.Uri)
            {
                if (r == null)
                    UriLoader.Load(g, new Uri(ontology.Location));
                else
                    UriLoader.Load(g, new Uri(ontology.Location), r);
            }
            else if (ontology.Type == RdfType.TTL)
            {
                if (ontology.Source == RdfSource.String)
                    r.Load(g, new StringReader(ontology.Location));
                else
                    r.Load(g, ontology.Location);
            }
            else
            {
                try
                {
                    FileLoader.Load(g, ontology.Location);
                }
                catch (Exception e)
                {
                    while (e.InnerException != null) e = e.InnerException;
                    throw new Exception($"Rdf Source and Type Unknown - {ontology.Location} - cannot load as TTL from file: {e.Message}");
                }
            }
            return g;
        }

        public static IEnumerable<Owl> GetOwlHierarchy(IGraph graph)
        {
            IUriNode rdfType = graph.CreateUriNode("rdf:type");
            IUriNode owlClass = graph.CreateUriNode("owl:Class");

            var triples = graph.GetTriplesWithPredicateObject(rdfType, owlClass);

            var processing = new List<FlatOwl>();

            // Phase 1: Get the data I need out of the Graph

            foreach (var t in triples)
            {
                IUriNode rdfsLabel = graph.CreateUriNode("rdfs:label");
                var possible_labels = GetValuesFromTriples(graph.GetTriplesWithSubjectPredicate(t.Subject, rdfsLabel));
                var label = possible_labels.FirstOrDefault() ?? t.Subject.ToString().Split('/').Reverse().First();

                IUriNode rdfsSubClassOf = graph.CreateUriNode("rdfs:subClassOf");
                var parents = graph.GetTriplesWithSubjectPredicate(t.Subject, rdfsSubClassOf);

                // remove any 'parents' that aren't also types in the current graph
                List<Triple> sanitisedParents = new List<Triple>();
                foreach(var p in parents)
                {
                    if (graph.ContainsTriple(new Triple(p.Object, rdfType, owlClass)))
                        sanitisedParents.Add(p);
                }

                foreach (var sp in GetValuesFromTriples(sanitisedParents))
                {
                    processing.Add(new FlatOwl(t.Subject.ToString(), label, sp));
                }
                
            }

            // Phase 2: Consolidate the data I need
            var ret = new List<Owl>();
            



            return ret;
        }
        

        public static IEnumerable<string> GetValuesFromTriples(IEnumerable<Triple> triples)
        {
            List<string> ret = new List<string>();
            foreach(var t in triples)
                ret.Add(t.Object.ToString());
            return ret;
        }

        #region Encapsulated POCOs 

        private class FlatOwl
        {
            public string Uri { get; set; }
            public string Label { get; set; }
            public string Parent { get; set; }

            public FlatOwl(string uri, string label, string parent)
            {
                Uri = uri;
                Label = label;
                Parent = parent;
            }
        }

        #endregion
    }
}
