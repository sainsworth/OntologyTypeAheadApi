using OntologyTypeAheadApi.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using OntologyTypeAheadApi.Models.Contract;
using OntologyTypeAheadApi.Models.Response;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using OntologyTypeAheadApi.Context.Contract;
using OntologyTypeAheadApi.Helpers;
using OntologyTypeAheadApi.Enums;

namespace OntologyTypeAheadApi.Service.Implementation
{
    public class DatastoreService : IDatastoreService
    {
        private IDatastoreContext _datastoreContext;
        private IRdfSourceContext _rdfSourceContext;
        public IResponse<IEnumerable<LookupItem>> QueryDatastore_Contains(string query)
        {
            var resp = new EnumerableResponse<LookupItem>();
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
                        resp.Data = data.OrderBy(x => x.Label);
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

        public IResponse PopulateDatastore()
        {
            var resp = new EmptyResponse();
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                foreach (var x in _rdfSourceContext.All())
                {
                    var graph = RdfHelper.GetGraphFromOntology(x);
                    var items = RdfHelper.GetFlatDataFromGraph(graph);

                    items.ToList().ForEach(y => data[y.Key] = y.Value);
                }
               _datastoreContext.Populate(data);
                resp.Status = ResponseStatus.DataLoaded;
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