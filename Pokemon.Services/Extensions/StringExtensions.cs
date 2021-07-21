using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pokemon.Services.Extensions
{
    public static class StringExtensions
    {
        public static string ToQueryString(this Dictionary<string, string> source)
        {
            return String.Join("&", source.Select(kvp => String.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }
    }
}
