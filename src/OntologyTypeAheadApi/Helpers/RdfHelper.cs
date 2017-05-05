using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace OntologyTypeAheadApi.Helpers
{

    public static class RdfHelper
    {
        public static IGraph GetGraphFromOntology(Ontology ontology)
        {
            IGraph g = new Graph();
            IRdfReader r = ontology.RdfType == RdfType.TTL ? new TurtleParser() : null;

            if (ontology.RdfSource == RdfSource.Uri)
            {
                if (r == null)
                    UriLoader.Load(g, new Uri(ontology.Location));
                else
                    UriLoader.Load(g, new Uri(ontology.Location), r);
            }
            else if (ontology.RdfType == RdfType.TTL)
            {
                if (ontology.RdfSource == RdfSource.String)
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

        public static Dictionary<string,string> GetFlatDataFromGraph(IEnumerable<string>rootTypes, IGraph graph)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            List<string> subjectList = new List<string>();

            var roots = getWhereSubclassOf(rootTypes, graph);
            var allroots = roots;
            while (roots.Count() > 0)
            {
                roots = getWhereSubclassOf(roots, graph);
                allroots = allroots.Concat(roots).ToList();
            }

            foreach (var r in allroots)
            {
                IUriNode rdfsLabel = graph.CreateUriNode("rdfs:label");
                IUriNode subject = graph.CreateUriNode(new Uri(r));
                var possible_labels = GetValuesFromTriples(graph.GetTriplesWithSubjectPredicate(subject, rdfsLabel));
                var label = possible_labels.FirstOrDefault() ?? r.Split('/').Reverse().First();

                ret[r] = label;
            }

            //IUriNode predNode = graph.CreateUriNode("rdf:type");
            //INode objNode = graph.CreateUriNode("owl:Class");
            //var triples = graph.GetTriplesWithPredicateObject(predNode, objNode);
            //foreach (var t in triples)
            //{
            //    IUriNode rdfsLabel = graph.CreateUriNode("rdfs:label");
            //    var possible_labels = GetValuesFromTriples(graph.GetTriplesWithSubjectPredicate(t.Subject, rdfsLabel));
            //    var label = possible_labels.FirstOrDefault() ?? t.Subject.ToString().Split('/').Reverse().First();

            //    ret[t.Subject.ToString()] = label;
            //}
            return ret;
        }
        
        private static IEnumerable<string> getWhereSubclassOf(IEnumerable<string> roots, IGraph graph)
        {
            var ret = new List<string>();
            foreach (var x in roots)
            {
                var q = @"PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
SELECT ?subject
WHERE { ?subject rdfs:subClassOf <" + x + "> }";
                var r = (SparqlResultSet)graph.ExecuteQuery(q);
                r.Results.ForEach(y =>
                {
                    for (int i = 0; i < y.Count(); i++)
                    {
                        ret.Add(y.Value("subject").ToString());
                    }
                });
            }
            return ret;
        }

        public static IEnumerable<string> GetValuesFromTriples(IEnumerable<Triple> triples)
        {
            List<string> ret = new List<string>();
            foreach(var t in triples)
                ret.Add(t.Object.ToString());
            return ret;
        }

       
    }
}
