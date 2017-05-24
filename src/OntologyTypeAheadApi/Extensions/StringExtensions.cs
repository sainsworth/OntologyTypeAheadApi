using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OntologyTypeAheadApi.Extensions
{
    public static class StringExtensions
    {
        public enum SanitiseStringMode
        {
            ToUpper,
            ToLower,
            KeepCase
        }
        public static string Sanitise(this string s, SanitiseStringMode mode = SanitiseStringMode.ToUpper, bool StripSpaces=true)
        {
            if (mode == SanitiseStringMode.ToUpper)
                s = s.ToUpper();
            else if (mode == SanitiseStringMode.ToLower)
                s = s.ToLower();

            s = s.Replace("-", "").Replace(",", "");
            if (StripSpaces) s = s.Replace(" ", "");
            return s;
        }

        public static string ElasticQueryStringEscaped(this string s)
        {
            s = s.Replace("\\", "\\\\");
            var replaceList = new List<string>() {
                "+"
                , "-"
                , "!"
                , "("
                , ")"
                , ":"
                , "^"
                , "["
                , "]"
                , "\""
                , "{"
                , "}"
                , "~"
                , "*"
                , "?"
                , "|"
                , "&"
                , "/"
            };
            replaceList.ForEach(x => s = s.Replace(x, $"\\{x}"));
            return s;
        }
    }
}