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
        IResponse<IEnumerable<LookupResponse>> GetMatches(string query);
        IResponse Populate();
    }
}
