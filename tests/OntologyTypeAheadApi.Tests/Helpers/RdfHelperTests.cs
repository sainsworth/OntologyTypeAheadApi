using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Models.Response;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Helpers;
using VDS.RDF;
using OntologyTypeAheadApi.Enums;
using System;

namespace OntologyTypeAheadApi.Tests.Helpers
{
    [TestClass]
    public class RdfHelperTests
    {
        public string testTtl = @"@prefix : <http://www.stew.test.uk/ontologytypeaheadapi/> .
@prefix owl: <http://www.w3.org/2002/07/owl#> .
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
@prefix xml: <http://www.w3.org/XML/1998/namespace> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .
@base <http://www.stew.test.uk/ontologytypeaheadapi> .

<http://www.stew.test.uk/ontologytypeaheadapi> rdf:type owl:Ontology .

<http://www.stew.test.uk/ontologytypeaheadapi/this> rdf:type owl:Class ;
                                                    rdfs:label ""This Label"" .
				
<http://www.stew.test.uk/ontologytypeaheadapi/that> rdf:type owl:Class .

<http://www.stew.test.uk/ontologytypeaheadapi/the_other> rdf:type owl:Class ;
                                                         rdfs:label ""The Other Label"" .

<http://www.stew.test.uk/ontologytypeaheadapi/child_of_this> rdf:type owl:Class ;
                                                             rdfs:subClassOf <http://www.stew.test.uk/ontologytypeaheadapi/this> ;
                                                             rdfs:label ""Child Of This Label"".
                                                             
";

        IGraph graph = null;

        [TestMethod]
        public void RdfHelper_LoadTtl_OK()
        {
            Exception e = null;
            try
            {
                graph = RdfHelper.GetGraphFromOntology(new Ontology(testTtl, RdfSource.String, RdfType.TTL));
            }
            catch (Exception ee)
            {
                e = ee;
            }

            Assert.AreEqual(null, e);
        }

        [TestMethod]
        public void RdfHelper_GetOwlHierarchy()
        {
            var expected = new List<Owl>()
            {
                new Owl() {
                    Uri = "http://www.stew.test.uk/ontologytypeaheadapi/this",
                    Label = "This Label",
                    Children = new List<Owl>()
                    {
                        new Owl()
                        {
                            Uri = "http://www.stew.test.uk/ontologytypeaheadapi/child_of_this",
                            Label = "Child Of This Label"
                        }
                    }
                },
                new Owl ()
                {
                    Uri = "http://www.stew.test.uk/ontologytypeaheadapi/that",
                    Label = "that"
                },
                new Owl ()
                {
                    Uri = "http://www.stew.test.uk/ontologytypeaheadapi/the_other",
                    Label = "The Other Label"
                }
            };

            // this will fail if RdfHelper_LoadTtl_OK() fails
            var result = RdfHelper.GetOwlHierarchy(RdfHelper.GetGraphFromOntology(new Ontology(testTtl, RdfSource.String, RdfType.TTL)));

            Assert.AreEqual(expected.AsEnumerable(), result);
        }
    }
}

