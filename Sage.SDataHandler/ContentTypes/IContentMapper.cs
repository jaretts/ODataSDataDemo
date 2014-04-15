using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    public interface IContentMapper
    {
        /// <summary>
        /// Provide custom mapping from the OData property name to an SData property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string Map(string propertyName);
    }
}
