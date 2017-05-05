using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Collections.Generic;

namespace OntologyTypeAheadApi.Context.Contract
{
    public interface IDatastoreContext
    {
        IEnumerable<LookupItem> All(string accessor);
        IEnumerable<LookupItem> Equals(string accessor,string query, bool casesensitive = false);
        IEnumerable<LookupItem> StartsWith(string accessor,string query, bool casesensitive = false);
        IEnumerable<LookupItem> Contains(string accessor,string query, bool casesensitive = false);
        void Populate(Dictionary<string,Dictionary<string,string>> data);
    }
}
