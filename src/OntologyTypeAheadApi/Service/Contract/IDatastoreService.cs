using OntologyTypeAheadApi.Models.Contract;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Service.Contract
{
    public interface IDatastoreService
    {
        Task<IResponse<IEnumerable<LookupItem>>> QueryDatastore_Contains(string accessor, string query);
        Task<IResponse<IEnumerable<LookupItem>>> QueryDatastore_All(string accessor);
        Task<IResponse> PopulateDatastore();
    }
}
