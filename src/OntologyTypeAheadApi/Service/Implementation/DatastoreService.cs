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
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Service.Implementation
{
    public class DatastoreService : IDatastoreService
    {
        private IDatastoreContext _datastoreContext;
        private IRdfSourceContext _rdfSourceContext;
        public async Task<IResponse<IEnumerable<LookupItem>>> QueryDatastore_Contains(string accessor, string query)
        {
            var resp = new EnumerableResponse<LookupItem>();
            if (string.IsNullOrWhiteSpace(query))
                resp.Status = Enums.ResponseStatus.NoResponse;
            else
            {
                try
                {
                    var data = await _datastoreContext.Contains(accessor, query);
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

        public async Task<IResponse<IEnumerable<LookupItem>>> QueryDatastore_All(string accessor)
        {
            var resp = new EnumerableResponse<LookupItem>();
            try
            {
                var data = await _datastoreContext.All(accessor);
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

            return resp;
        }

        public async Task<IResponse> PopulateDatastore()
        {
            var resp = new EnumerableResponse<dynamic>();
            var accessors = new Dictionary<string, string>();
            var r = new List<LoadOntologyResponse>();

            Dictionary<string,Dictionary<string, string>> data = new Dictionary<string,Dictionary<string, string>>();
            try
            {
                var finalStatus = ResponseStatus.DataLoaded;
                var d = _rdfSourceContext.All();
                foreach (var x in d)
                {
                    try
                    {
                        accessors[x.Accessor] = x.Label;
                        var graph = RdfHelper.GetGraphFromOntology(x);
                        var items = RdfHelper.GetFlatDataFromGraph(x.RootTypes, graph);

                        data[x.Accessor] = items;
                        r.Add(new LoadOntologyResponse(x.Location));
                    }
                    catch (Exception e)
                    {
                        r.Add(new LoadOntologyResponse(x.Location, ResponseStatus.Error, e));
                        finalStatus = ResponseStatus.Error;
                        resp.Message = "One or more ontologies could not be loaded";
                    }
                }
                await _datastoreContext.Populate(accessors, data);
                resp.Status = finalStatus;
            }
            catch (Exception e)
            {
                resp.Status = Enums.ResponseStatus.Error;
                resp.Exception = e;
            }
            resp.Data = r;
            return resp;
        }

        public DatastoreService(IDatastoreContext datastoreContext, IRdfSourceContext rdfSourceContext)
        {
            _datastoreContext = datastoreContext;
            _rdfSourceContext = rdfSourceContext;
        }
    }
}