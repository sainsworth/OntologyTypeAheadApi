using OntologyTypeAheadApi.Models.Contract;

namespace OntologyTypeAheadApi.Models.Request
{
    public class ConformedRequest: IRequest
    {
        public string Route { get; set; }
        public dynamic Details { get; set; }
    }
}