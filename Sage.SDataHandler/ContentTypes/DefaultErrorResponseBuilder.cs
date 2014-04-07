using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sage.SDataHandler.ContentTypes
{
    public class DefaultErrorResponseBuilder : IErrorResponseBuilder
    {

        public HttpResponseMessage BuildErrorResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.Content is ObjectContent<System.Web.Http.HttpError>)
            {
                ContentNegotiationResult result = FindContentNegotiation(request);

                if (result != null)
                {
                    var origStatusCode = response.StatusCode;
                    ObjectContent<System.Web.Http.HttpError> origErrorContent = (ObjectContent<System.Web.Http.HttpError>)response.Content;
                    HttpError errorVal = origErrorContent.Value as HttpError;

                    SDataDiagnosis errorContent = new SDataDiagnosis()
                    {
                        severity = "Error",
                        message = errorVal.ExceptionMessage + " " + errorVal.MessageDetail,
                        stackTrace = errorVal.StackTrace
                    };

                    HttpResponseMessage errorResponse = new HttpResponseMessage()
                    {
                        StatusCode = origStatusCode,
                        Content = new ObjectContent<SDataDiagnosis>(
                            errorContent,
                            result.Formatter,
                            result.MediaType.MediaType)
                    };
                    response.Headers.ToList().ForEach(i => errorResponse.Headers.Add(i.Key, i.Value));

                    response = errorResponse;
                }
            }

            return response;
        }

        private static ContentNegotiationResult FindContentNegotiation(HttpRequestMessage request)
        {
            IContentNegotiator negotiator = GlobalConfiguration.Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult result = negotiator.Negotiate(
                   typeof(SDataDiagnosis), request, GlobalConfiguration.Configuration.Formatters);

            return result;
        }
   
    }
}
