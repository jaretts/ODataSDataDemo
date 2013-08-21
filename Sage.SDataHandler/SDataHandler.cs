using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Tokens.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Sage.SDataHandler
{
    public class SDataHandler : DelegatingHandler //AuthenticationHandler
    {

        public SDataHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }

        public SDataHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {

            Uri originalUri = request.RequestUri;

            // check if a param like format=json was in original uri and set Accept header
            //SDataUriUtil.SetAcceptJsonHeader(request, originalUri);

            // convert any SData query keys (where, startIndex, etc.) 
            Uri newUri = SDataUriUtil.TranslateUri(originalUri, SDataUriKeys.CONVERT_TO_ODATA);

            request.RequestUri = newUri;

            var response = await base.SendAsync(request, cancellationToken);

            if (ResponseIsValid(response))
            {
                object responseObject;
                response.TryGetContentValue(out responseObject);

                if (responseObject is IQueryable)
                {
                    var old = await response.Content.ReadAsStringAsync();

                    string nwContent = old.Replace("\"value\":[", "\"$resources\":[");
                    nwContent = nwContent.Replace("odata.count", "$totalResults");
                    nwContent = nwContent.Replace("odata.metadata", "$Url");
                    nwContent = nwContent.Replace("$metadata#", "");

                    response.Content = new StringContent(nwContent);
                }
            }

            return response;
        }
        

        private bool ResponseIsValid(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                return false;
            }

            if (response == null || !(response.Content is ObjectContent))
            {
                return false;
            }
            
            return true;
        }

        }

}