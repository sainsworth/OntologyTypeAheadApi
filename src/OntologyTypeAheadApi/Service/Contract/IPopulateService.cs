using OntologyTypeAheadApi.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Service.Contract
{
    public interface IPopulateService
    {
        Task<IResponse> PopulateDatastore();
    }
}
