using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    public interface IErrorResponseBuilder
    {
        HttpResponseMessage BuildErrorResponse(HttpRequestMessage request, HttpResponseMessage response);
    }
}
