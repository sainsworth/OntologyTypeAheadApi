using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OntologyTypeAheadApi.Tests.Helpers;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Helpers;
using VDS.RDF;
using OntologyTypeAheadApi.Enums;
using System;
using System.Linq;

namespace OntologyTypeAheadApi.Tests.HelperTests
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

<http://www.stew.test.uk/ontologytypeaheadapi/anotherroot> rdf:type owl:Class ;
                                                    rdfs:label ""Some Other Root Label"" .

<http://www.stew.test.uk/ontologytypeaheadapi/root> rdf:type owl:Class ;
                                                    rdfs:label ""Root Label"" .

<http://www.stew.test.uk/ontologytypeaheadapi/this> rdf:type owl:Class ;
                                                    rdfs:subClassOf <http://www.stew.test.uk/ontologytypeaheadapi/root> ;
                                                    rdfs:label ""This Label"" .
				
<http://www.stew.test.uk/ontologytypeaheadapi/that> rdf:type owl:Class ;
                                                    rdfs:subClassOf <http://www.stew.test.uk/ontologytypeaheadapi/root> .

<http://www.stew.test.uk/ontologytypeaheadapi/the_other> rdf:type owl:Class ;
                                                         rdfs:subClassOf <http://www.stew.test.uk/ontologytypeaheadapi/root> ;
                                                         rdfs:label ""The Other Label"" .

<http://www.stew.test.uk/ontologytypeaheadapi/child_of_this> rdf:type owl:Class ;
                                                             rdfs:subClassOf <http://www.stew.test.uk/ontologytypeaheadapi/this> ;
                                                             rdfs:label ""Child Of This Label"".
                                                             
";

        [TestMethod]
        public void RdfHelper_GetLookupItems()
        {
            var expected = new Dictionary<string, string>()
            {
                { "http://www.stew.test.uk/ontologytypeaheadapi/this", "This Label" },
                { "http://www.stew.test.uk/ontologytypeaheadapi/that", "that" },
                { "http://www.stew.test.uk/ontologytypeaheadapi/the_other","The Other Label" },
                { "http://www.stew.test.uk/ontologytypeaheadapi/child_of_this", "Child Of This Label" }
            };

            // this will fail if RdfHelper_LoadTtl_OK() fails

            Exception e = null;
            IGraph graph = null;

            var ontology = new Ontology(
                               testTtl,
                               "test",
                               "Test Label",
                               new List<string>() { "http://www.stew.test.uk/ontologytypeaheadapi/root" },
                               RdfSource.String, RdfType.TTL
                           );
            try
            {
                graph = RdfHelper.GetGraphFromOntology(ontology);
            }
            catch (Exception ee)
            {
                e = ee;
            }

            Assert.AreEqual(null, e);

            var result = RdfHelper.GetFlatDataFromGraph(ontology.RootTypes, graph);

            AssertHelper.DictionariesAreEqual(expected, result);
        }
    }
}

