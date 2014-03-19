using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Sage.SDataHandler.Uris
{
    public class SDataUriUtil
    {
        private const string PARAM_SEP = "&";
        private const string URL_QUERY_SEP = "?";

        public static Uri TranslateUri(Uri requestUri)
        {
            NameValueCollection query = requestUri.ParseQueryString();
            bool isSingleResourceRequest = IsSingleResourceRequest(requestUri);
            string translatedUriQuery = "";

            // Scan every parameter and convert parameter name OData
            foreach (string key in query.AllKeys)
            {
                // check for startIndex. StartIndex require decrementing value because OData is 0 based
                ConvertStartIndexValue(query, key);

                // convert known SData parameter names to OData parameter names; e.g startIndex to $skip
                string translatedKey = ConvertSDataKeyToODataKey(key);

                if (translatedUriQuery.Length > 0)
                {
                    // need to append sep
                    translatedUriQuery += PARAM_SEP;
                }

                // Add the param name and value to the new query portion we're building
                translatedUriQuery += ReplaceParamName(translatedKey, query[key]);
            }

            // make sure URL contains $inlinecount param so payload contains count
            if (!isSingleResourceRequest && !translatedUriQuery.Contains("$inlinecount"))
            {
                translatedUriQuery = AddParam(translatedUriQuery, "$inlinecount", "allpages");
            }

            Uri retValue = ReplaceQueryPortionOfUri(requestUri, translatedUriQuery);

            return retValue;
        }

        private static bool IsSingleResourceRequest(Uri requestUri)
        {
            string rPath = requestUri.AbsolutePath;
            int idxResourceSelector = rPath.IndexOf('(');
            bool isSingleResourceRequest = idxResourceSelector > 0 && rPath.IndexOf(')') > idxResourceSelector;
            return isSingleResourceRequest;
        }

        private static void ConvertStartIndexValue(NameValueCollection query, string key)
        {
            if (key.Equals(SDataUriKeys.SDATA_STARTINDEX, StringComparison.InvariantCultureIgnoreCase))
            {
                //StartIndex require decrementing value because OData is 0 based
                int startIndx = int.Parse(query[key]);
                startIndx = Math.Max(startIndx - 1, 0);
                query[key] = "" + startIndx;
            }
        }

        private static string ConvertSDataKeyToODataKey(string key)
        {
            string translatedKey;
            if (SDataUriKeys.ODATA_DICTIONARY.ContainsKey(key))
            {
                translatedKey = SDataUriKeys.ODATA_DICTIONARY[key];
            }
            else
            {
                // this is not a recognized key so leave it alone
                translatedKey = key;
            }

            return translatedKey;
        }

        private static Uri ReplaceQueryPortionOfUri(Uri requestUri, string translatedQuery)
        {
            Uri retValue;
            if (string.IsNullOrEmpty(translatedQuery))
            {
                // no changes required
                retValue = requestUri;
            }
            else
            {
                // there were params to map from sdata to odata so strip off old uri query 
                string originalUri = requestUri.AbsoluteUri;
                if (originalUri.IndexOf(URL_QUERY_SEP) > 0)
                {
                    originalUri = originalUri.Substring(0, originalUri.IndexOf(URL_QUERY_SEP) );
                }

                if (!translatedQuery.StartsWith(URL_QUERY_SEP))
                {
                    translatedQuery = URL_QUERY_SEP + translatedQuery;
                }

                retValue = new Uri(originalUri + translatedQuery);
            }

            return retValue;
        }

        private static string ReplaceParamName(string translatedKey, string value)
        {
            string retValue = "";

            if (!String.IsNullOrEmpty(translatedKey))
            {
                 retValue = translatedKey + "=" + value;
            }

            return retValue;
        }

        private static string AddParam(string translatedUriQuery, string key, string value)
        {
            // add "$inlinecount=allpages" so total count will appear in results
            if (translatedUriQuery.Length > 0)
            {
                // need to append sep
                translatedUriQuery += PARAM_SEP;
            }

            translatedUriQuery += key + "=" + value;

            return translatedUriQuery;
        }

    }
}