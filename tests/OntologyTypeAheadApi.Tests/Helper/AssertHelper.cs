using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace OntologyTypeAheadApi.Tests.Helpers
{
    public static class AssertHelper
    {
        public static void ListsAreEqual<T>(IEnumerable<IEquatable<T>> ea, IEnumerable<IEquatable<T>> eb)
        {
            if (ea == null && eb == null)
                return;
            if (ea == null || eb == null)
                throw new AssertFailedException("One list is null where as the other is not");
            if (ea.Count() != eb.Count())
                throw new AssertFailedException("The supplied lists are not the same length");

            for (int i = 0; i < ea.Count(); i++)
                if (!ea.ElementAt(i).Equals(eb.ElementAt(i)))
                    throw new AssertFailedException($"The lists differ ar element {i}");
        }

        public static void DictionariesAreEqual<T1,T2>(Dictionary<T1, T2> da, Dictionary<T1,T2> db)
        {
            if (da == null && db == null)
                return;
            if (da == null || db == null)
                throw new AssertFailedException("One dictionary is null where as the other is not");
            if (da.Count() != db.Count())
                throw new AssertFailedException("The supplied lists are not the same length");

            foreach(var k in da.Keys)
                if (!db.ContainsKey(k))
                    throw new AssertFailedException($"The second dictionary does not contain the key {k.ToString()}");
                else if (!da[k].Equals(db[k]))
                    throw new AssertFailedException($"For key {k.ToString()} the dictionaries have different values");
        }
    }
}
