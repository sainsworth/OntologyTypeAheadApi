using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Models.Response.DataResponse
{
    public class LookupItem : IEquatable<LookupItem>
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public LookupItem(string id, string label)
        {
            Id = id;
            Label = label;
        }

        public bool Equals(LookupItem other)
        {
            if (other == null)
                return false;
            return (other.Id == Id && other.Label == Label);

        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LookupItem);
        }
    }
}