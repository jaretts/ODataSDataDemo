using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace Sage.SDataHandler.Uris
{
    public class SDataUriKeys
    {
        public const String SDATA_STARTINDEX = "startindex";
        public const String ODATA_STARTINDEX = "$skip";

        public const String SDATA_COUNT = "count";
        public const String ODATA_COUNT = "$top";

        public const String SDATA_ORDERBY = "orderby";
        public const String ODATA_ORDERBY = "$orderby";

        public const String SDATA_INCLUDE = "include";
        public const String ODATA_INCLUDE = "$expand";

        public const String SDATA_WHERE = "where";
        public const String ODATA_WHERE = "$filter";

        public const String SDATA_SELECT = "select";
        public const String ODATA_SELECT = "$select";

        public static readonly ReadOnlyDictionary<string, string> ODATA_DICTIONARY
                    = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { SDATA_STARTINDEX, ODATA_STARTINDEX },
                        { SDATA_COUNT, ODATA_COUNT },
                        { SDATA_ORDERBY, ODATA_ORDERBY },
                        { SDATA_INCLUDE, ODATA_INCLUDE },
                        { SDATA_WHERE, ODATA_WHERE },
                        { SDATA_SELECT, ODATA_SELECT },
                    }
        );

    }       
}