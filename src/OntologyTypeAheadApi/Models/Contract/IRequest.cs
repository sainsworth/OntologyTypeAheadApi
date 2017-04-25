using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Models.Contract
{
    public interface IRequest
    {
        string Route { get; set; }
        dynamic Details { get; set; }
    }
}
