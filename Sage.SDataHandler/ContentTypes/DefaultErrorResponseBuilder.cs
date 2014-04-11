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

        public virtual HttpResponseMessage BuildErrorResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.Content is ObjectContent<System.Web.Http.HttpError>)
            {
                ContentNegotiationResult result = FindContentNegotiation(request);

                if (result != null)
                {
                    ObjectContent<System.Web.Http.HttpError> origErrorContent = (ObjectContent<System.Web.Http.HttpError>)response.Content;
                    HttpError errorVal = origErrorContent.Value as HttpError;

                    response = BuildDiagnosisResponse(response, 
                                        result, 
                                        "", errorVal.ExceptionMessage + " " + errorVal.MessageDetail,
                                        errorVal.StackTrace);
                }
            }

            return response;
        }

        protected static HttpResponseMessage BuildDiagnosisResponse(HttpResponseMessage response, 
                                               ContentNegotiationResult result, string appCode, string errMessage, string errStackTrace)
        {
            var origStatusCode = response.StatusCode;
            SDataDiagnosis errorContent = new SDataDiagnosis()
            {
                severity = "Error",
                applicationCode = appCode,
                message = errMessage,
                stackTrace = errStackTrace
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
            return errorResponse;
        }

        public static ContentNegotiationResult FindContentNegotiation(HttpRequestMessage request)
        {
            IContentNegotiator negotiator = GlobalConfiguration.Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult result = negotiator.Negotiate(
                   typeof(SDataDiagnosis), request, GlobalConfiguration.Configuration.Formatters);

            return result;
        }
   
    }
}
