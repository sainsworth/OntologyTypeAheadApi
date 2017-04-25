using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Collections.Generic;

namespace OntologyTypeAheadApi.Context.Contract
{
    public interface IDatastoreContext
    {
        IEnumerable<LookupItem> All();
        IEnumerable<LookupItem> Equals(string query, bool casesensitive = false);
        IEnumerable<LookupItem> StartsWith(string query, bool casesensitive = false);
        IEnumerable<LookupItem> Contains(string query, bool casesensitive = false);
    }
}
