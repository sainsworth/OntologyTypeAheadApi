﻿using OntologyTypeAheadApi.Context.Contract;
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
        private Dictionary<string,IEnumerable<LookupItem>> _data;
        private Dictionary<string, string> _accessors;

        public Mock_DatastoreContext()
        {
            _data = SetupMockData();
        }

        public Mock_DatastoreContext(Dictionary<string, IEnumerable<LookupItem>> data)
        {
            _data = data;
        }

        public async Task<IEnumerable<LookupItem>> All(string accessor)
        {
            if (_data.ContainsKey(accessor.ToLowerInvariant()))
                return await Task.Run(() => _data[accessor.ToLowerInvariant()]);
            return null;
        }

        public async Task<IEnumerable<LookupItem>> Contains(string accessor, string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            if (_data.ContainsKey(accessor.ToLowerInvariant()))
                return await Task.Run(() => _data[accessor.ToLowerInvariant()].Where(x => x.Label.Sanitise(mode).Contains(query.Sanitise(mode))).AsEnumerable());
            else
                return null;
        }

        public async Task<IEnumerable<LookupItem>> Equals(string accessor, string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            if (_data.ContainsKey(accessor.ToLowerInvariant()))
                return await Task.Run(() => _data[accessor.ToLowerInvariant()].Where(x => x.Label.Sanitise(mode) == query.Sanitise(mode)).AsEnumerable());
            else
                return null;
        }

        public async Task<IEnumerable<LookupItem>> StartsWith(string accessor, string query, bool casesensitive = false)
        {
            var mode = _sanitiseStringMode(casesensitive);
            if (_data.ContainsKey(accessor.ToLowerInvariant()))
                return await Task.Run(() => _data[accessor.ToLowerInvariant()].Where(x => x.Label.Sanitise(mode).StartsWith(query.Sanitise(mode))).AsEnumerable());
            else
                return null;
        }

        private StringExtensions.SanitiseStringMode _sanitiseStringMode(bool casesensitive)
        {
            return casesensitive ? StringExtensions.SanitiseStringMode.KeepCase : StringExtensions.SanitiseStringMode.ToUpper;
        }

        private Dictionary<string, IEnumerable<LookupItem>> SetupMockData()
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
            var data = new List<LookupItem>();
            for (int i = 0; i < towns.Count; i++)
                data.Add(new LookupItem(i.ToString(), towns[i]));
            var ret = new Dictionary<string, IEnumerable<LookupItem>>();
            ret.Add("mock", data);
            return ret;
        }

        public async Task Populate(Dictionary<string, string> accessors, Dictionary<string, Dictionary<string, string>> data)
        {
            _data = new Dictionary<string, IEnumerable<LookupItem>>();
            _accessors = accessors;
            foreach (var d in data)
            {
                var thisdata = new List<LookupItem>();
                d.Value.ToList().ForEach(x => thisdata.Add(new LookupItem(x.Key, x.Value)));
                await Task.Run(() => _data[d.Key] = thisdata);
            }
        }
    }
}

