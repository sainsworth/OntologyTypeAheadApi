using Newtonsoft.Json;
using OntologyTypeAheadApi.Enums;
using OntologyTypeAheadApi.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response
{
    public class EnumerableResponse<T> :IResponse<IEnumerable<T>>
    {
        #region public properties

        public IEnumerable<T> Data { get; set; }
        [JsonIgnore]
        public ResponseStatus Status { get; set; }
        [JsonIgnore]
        private Exception _exception;
        [JsonIgnore]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                if (value == null)
                {
                    _exception = null;
                    _innerException = null;
                }
                else
                {
                    _exception = value;
                    _innerException = value;
                    while (_innerException.InnerException != null)
                        _innerException = _innerException.InnerException;
                }
            }
        }
        
        private Exception _innerException;
        [JsonIgnore]
        public Exception InnerException { get
            {
                return _innerException;
            }
        }
        [JsonProperty(PropertyName = "Exception")]
        public string ExceptionMessage {
            get
            {
                if (_innerException != null)
                    return _innerException.Message;
                return null;
            }
        }
        public string Message { get; set; }
        #endregion

        #region constructors

        public EnumerableResponse()
        {
            Status = ResponseStatus.NotSet;
        }
        
        #endregion
    }
}
