using OntologyTypeAheadApi.Context.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OntologyTypeAheadApi.Extensions;
using OntologyTypeAheadApi.Models.Response.DataResponse;
using System.Threading.Tasks;

namespace OntologyTypeAheadApi.Context.Implementation
{
    public class Mock_DatastoreContext : IDatastoreContext
    {
        private IEnumerable<LookupItem> _data;

        public Mock_DatastoreContext()
        {
            _data = SetupMockData();
        }

        public IEnumerable<LookupItem> All()
        {
            return _data;
        }

        public IEnumerable<LookupItem> Contains(string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            return _data.Where(x => x.Label.Sanitise(mode).Contains(query.Sanitise(mode))).AsEnumerable();
        }

        public IEnumerable<LookupItem> Equals(string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            return _data.Where(x => x.Label.Sanitise(mode) == query.Sanitise(mode)).AsEnumerable();
        }

        public IEnumerable<LookupItem> StartsWith(string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            return _data.Where(x => x.Label.Sanitise(mode).StartsWith(query.Sanitise(mode))).AsEnumerable();
        }

        private StringExtensions.SanitiseStringMode _sanitiseStringMode(bool casesensitive)
        {
            return casesensitive ? StringExtensions.SanitiseStringMode.KeepCase : StringExtensions.SanitiseStringMode.ToUpper;
        }

        private IEnumerable<LookupItem> SetupMockData()
        {
            var towns = new List<string>() {
                "Abingdon-on-Thames",
                "Accrington",
                "Acle",
                "Acton",
                "Adlington",
                "Alcester",
                "Aldeburgh",
                "Aldershot",
                "Alford",
                "Alfreton",
                "Alnwick",
                "Alsager",
                "Alston",
                "Alton",
                "Altrincham",
                "Amble",
                "Ambleside",
                "Amersham",
                "Amesbury",
                "Ampthill",
                "Andover",
                "Appleby-in-Westmorland",
                "Arlesey",
                "Arundel",
                "Ashbourne",
                "Ashburton",
                "Ashby Woulds",
                "Ashby-de-la-Zouch",
                "Ashford",
                "Ashington",
                "Ashton-under-Lyne",
                "Askern",
                "Aspatria",
                "Atherstone",
                "Attleborough",
                "Axbridge",
                "Axminster",
                "Aylesbury",
                "Aylsham"
            };
            var ret = new List<LookupItem>();
            for(int i = 0; i < towns.Count; i++)
                ret.Add(new LookupItem(i.ToString(), towns[i]));
            return ret;
        }

        public void Populate(Dictionary<string,string> data)
        {
            var newData = new List<LookupItem>();
            data.ToList().ForEach(x => newData.Add(new LookupItem(x.Key, x.Value)));
            _data = newData;
        }

        public Mock_DatastoreContext(IEnumerable<LookupItem> data)
        {
            _data = data;
        }
    }
}