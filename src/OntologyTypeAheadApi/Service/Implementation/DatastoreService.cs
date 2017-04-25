using OntologyTypeAheadApi.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Models.Contract;
using OntologyTypeAheadApi.Models.Response;
using OntologyTypeAheadApi.Models.Response.Envelope;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Threading.Tasks;
using OntologyTypeAheadApi.Context.Contract;
using VDS.RDF;
using VDS.RDF.Parsing;
using OntologyTypeAheadApi.Enums;

namespace OntologyTypeAheadApi.Service.Implementation
{
    public class DatastoreService : IDatastoreService
    {
        private IDatastoreContext _datastoreContext;
        private IRdfSourceContext _rdfSourceContext;
        public IResponse<IEnumerable<LookupResponse>> GetMatches(string query)
        {
            var resp = new EnumerableResponse<LookupResponse>();
            if (string.IsNullOrWhiteSpace(query))
                resp.Status = Enums.ResponseStatus.NoResponse;
            else
            {
                try
                {
                    var data = _datastoreContext.Contains(query);
                    if (data == null || (data != null && data.Count() == 0))
                        resp.Status = Enums.ResponseStatus.NoResponse;
                    else
                    {
                        resp.Data = data;
                        resp.Status = Enums.ResponseStatus.OK;
                    }
                }
                catch (Exception e)
                {
                    resp.Status = Enums.ResponseStatus.Error;
                    resp.Exception = e;
                }
            }

            return resp;
        }

        public IResponse Populate()
        {
            var resp = new EmptyResponse();

            try
            {
                foreach (var x in _rdfSourceContext.All())
                {
                    IGraph g = new Graph();
                    IRdfReader r = null;

                    switch (x.Type)
                    {
                        case RdfType.TTL:
                            r = new TurtleParser();
                            break;
                    }

                    if (x.Source == RdfSource.Uri)
                    {
                        if (r == null)
                            UriLoader.Load(g, new Uri(x.Location));
                        else
                            UriLoader.Load(g, new Uri(x.Location), r);
                    }
                    else if (x.Type == RdfType.TTL)
                    {
                        r.Load(g, x.Location);
                    }
                    else
                    {
                        try
                        {
                            FileLoader.Load(g, x.Location);
                        }
                        catch (Exception e)
                        {
                            while (e.InnerException != null) e = e.InnerException;
                            throw new Exception($"Rdf Source and Type Unknown - {x.Location} - cannot load as TTL from file: {e.Message}");
                        }
                    }

                    var y = g.Triples;
                    
                }
                //resp.Status = ResponseStatus.DataLoaded;
            }
            catch (Exception e)
            {
                resp.Status = Enums.ResponseStatus.Error;
                resp.Exception = e;
            }

            return resp;
        }


        

        public DatastoreService(IDatastoreContext datastoreContext, IRdfSourceContext rdfSourceContext)
        {
            _datastoreContext = datastoreContext;
            _rdfSourceContext = rdfSourceContext;
        }
    }
}