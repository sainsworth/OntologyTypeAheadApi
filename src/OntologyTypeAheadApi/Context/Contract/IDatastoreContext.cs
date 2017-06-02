using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Context.Contract
{
    public interface IDatastoreContext
    {
        Task<IEnumerable<LookupItem>> All(string accessor);
        Task<IEnumerable<LookupItem>> Equals(string accessor,string query, bool casesensitive = false);
        Task<IEnumerable<LookupItem>> StartsWith(string accessor,string query, bool casesensitive = false);
        Task<IEnumerable<LookupItem>> Contains(string accessor,string query, bool casesensitive = false);
        Task Populate(Dictionary<string,string> accessors, Dictionary<string,Dictionary<string,string>> data);
    }
}
