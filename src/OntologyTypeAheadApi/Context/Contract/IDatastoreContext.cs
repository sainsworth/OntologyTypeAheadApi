using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Collections.Generic;

namespace OntologyTypeAheadApi.Context.Contract
{
    public interface IDatastoreContext
    {
        IEnumerable<LookupResponse> All();
        IEnumerable<LookupResponse> Equals(string query, bool casesensitive = false);
        IEnumerable<LookupResponse> StartsWith(string query, bool casesensitive = false);
        IEnumerable<LookupResponse> Contains(string query, bool casesensitive = false);
    }
}
