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
using System.Text;

namespace Sage.SDataHandler
{
    public class SDataHandler : DelegatingHandler //AuthenticationHandler
    {
        private const string ODATA_ELEMENT_KEY = "@Element\",";

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

            var acceptHeader = request.Headers.Accept;

            if (ResponseIsValid(response) && acceptHeader.Any(x => x.MediaType == "application/json"))
            {
                object responseObject;
                response.TryGetContentValue(out responseObject);

                var origContent = await response.Content.ReadAsStringAsync();

                StringBuilder nwContent = new StringBuilder(origContent);

                if (responseObject is IQueryable)
                {
                    nwContent = nwContent.Replace("\"value\":[", "\"$resources\":[");
                    nwContent = nwContent.Replace("odata.count", "$totalResults");
                    nwContent = nwContent.Replace("odata.metadata", "$url");
                    nwContent = nwContent.Replace("$metadata#", "");
                    nwContent = nwContent.Replace("$skip", "startindex");
                    nwContent = nwContent.Replace("odata.nextLink", "next");
                    nwContent = nwContent.Replace("$orderby", "orderby");
                }
                else if (origContent.Contains(ODATA_ELEMENT_KEY))
                {
                    int posmetaend = origContent.IndexOf(ODATA_ELEMENT_KEY);
                    nwContent = nwContent.Remove(0, posmetaend + ODATA_ELEMENT_KEY.Length);
                    nwContent = nwContent.Insert(0, "{\n");
                }

                if (nwContent.Length > 0)
                {
                    //System.Json
                    response.Content = new StringContent(nwContent.ToString());
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