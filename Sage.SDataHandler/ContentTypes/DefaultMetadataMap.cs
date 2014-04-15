using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    public class DefaultMetadataMap : IContentMapper
    {
        // Eliminate this as soon as support for property name Aliasing is supported
        private const string SDATA_METADATA_PROPNAME_PREFIX = "__SDataMetadata__";
        private const string SDATA_METADATA_ACTUAL_PREFIX = "$";

        public string Map(string propertyName)
        {
            // this is required until property name aliasing is supported by WebAPI/OData serializer
            // see: https://aspnetwebstack.codeplex.com/discussions/462757
            if (propertyName.Contains(SDATA_METADATA_PROPNAME_PREFIX))
                propertyName = propertyName.Replace(SDATA_METADATA_PROPNAME_PREFIX, SDATA_METADATA_ACTUAL_PREFIX);

            return propertyName;
        }
    }
}
