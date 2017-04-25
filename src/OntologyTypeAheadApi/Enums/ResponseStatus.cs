using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Enums
{
    public enum ResponseStatus
    {
        NotSet,

        OK,
        NoResponse,
        DataLoaded,

        InvalidRequest,
        Error
    }
}