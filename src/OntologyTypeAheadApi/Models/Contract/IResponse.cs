using OntologyTypeAheadApi.Enums;
using System;

namespace OntologyTypeAheadApi.Models.Contract
{

    public interface IResponse
    {
        ResponseStatus Status { get; set; }
        Exception Exception { get; set; }
        string Message { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T Data { get; set; }
    }
}
