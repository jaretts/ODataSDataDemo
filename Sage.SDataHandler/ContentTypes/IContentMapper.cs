using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    public interface IContentMapper
    {
        string Map(string oldValue);
    }
}
