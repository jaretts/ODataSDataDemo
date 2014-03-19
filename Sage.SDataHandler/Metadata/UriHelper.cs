using Sage.SDataHandler.Uris;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Sage.SDataHandler.Metadata
{
    static class UriHelper
    {
        public static int GetSDataStartIndexValue(Uri aUri)
        {
            NameValueCollection query = aUri.ParseQueryString();//. HttpUtility.ParseQueryString(aUri.Query);
            String retVal = ExtractParamFromQuery(query, SDataUriKeys.SDATA_STARTINDEX, SDataUriKeys.ODATA_STARTINDEX);

            if (String.IsNullOrEmpty(retVal))
            {
                return 1;
            }
            else
            {
                return int.Parse(retVal);
            }

        }

        public static int GetSDataCountValue(Uri aUri)
        {
            NameValueCollection query = aUri.ParseQueryString(); //HttpUtility.ParseQueryString(aUri.Query);
            String retVal = ExtractParamFromQuery(query, SDataUriKeys.SDATA_COUNT, SDataUriKeys.ODATA_COUNT);

            if (String.IsNullOrEmpty(retVal))
            {
                return -1;
            }
            else
            {
                return int.Parse(retVal);
            }

        }

        private static string ExtractParamFromQuery(NameValueCollection query, String sDataKey, String oDataKey)
        {
            String retVal = null;

            if (query.AllKeys.Contains(sDataKey))
            {
                retVal = query[sDataKey];
            }
            else if (query.AllKeys.Contains(oDataKey))
            {
                retVal = query[oDataKey];
            }

            if (string.IsNullOrWhiteSpace(retVal))
            {
                retVal = "";
            }

            return retVal;
        }

        /// <summary>
        /// Constructs a NameValueCollection into a query string.
        /// </summary>
        /// <remarks>Consider this method to be the opposite of "System.Web.HttpUtility.ParseQueryString"</remarks>
        /// <param name="parameters">The NameValueCollection</param>
        /// <param name="delimiter">The String to delimit the key/value pairs</param>
        /// <returns>A key/value structured query string, delimited by the specified String</returns>
        public static string ToQueryString(NameValueCollection parameters, String delimiter, Boolean omitEmpty)
        {
            if (String.IsNullOrEmpty(delimiter))
                delimiter = "&";
            Char equals = '=';
            List<String> items = new List<String>();
            for (int i = 0; i < parameters.Count; i++)
            {
                foreach (String value in parameters.GetValues(i))
                {
                    Boolean addValue = (omitEmpty) ? !String.IsNullOrEmpty(value) : true;
                    if (addValue)
                        items.Add(String.Concat(parameters.GetKey(i), equals, HttpUtility.UrlEncode(value)));
                }
            }

            return String.Join(delimiter, items.ToArray());
        }
    }
}
