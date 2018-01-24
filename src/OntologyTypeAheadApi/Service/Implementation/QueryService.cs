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
    public class QueryService : IQueryService
    {
        private IDatastoreContext _datastoreContext;

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

       
        public QueryService(IDatastoreContext datastoreContext, IRdfSourceContext rdfSourceContext)
        {
            _datastoreContext = datastoreContext;
        }
    }
}