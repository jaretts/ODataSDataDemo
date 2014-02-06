using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Text;
using Sage.SDataHandler.JsonHelpers;
using System.Collections;

namespace Sage.SDataHandler
{
    public class SDataHandler : DelegatingHandler //AuthenticationHandler
    {
        private const string ODATA_ELEMENT_KEY = "@Element\",";
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string SDATA_MEDIA_TYPE_PARAM_VND = "vnd.sage";
        private const string SDATA_MEDIA_TYPE_VALUE_VND = "sdata";
        private const string SDATA_MEDIA_TYPE_VALUE_FORMATTED = "formatted";

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
            var acceptHeader = request.Headers.Accept;
            bool acceptsSData = acceptHeader.Any(x => x.MediaType == JSON_MEDIA_TYPE &&
                                    x.Parameters.Any(p => p.Value == SDATA_MEDIA_TYPE_VALUE_VND) );

            if (!acceptsSData)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            Uri originalUri = request.RequestUri;

            // check if a param like format=json was in original uri and set Accept header
            //SDataUriUtil.SetAcceptJsonHeader(request, originalUri);

            // convert any SData query keys (where, startIndex, etc.) 
            Uri newUri = SDataUriUtil.TranslateUri(originalUri, SDataUriKeys.CONVERT_TO_ODATA);

            request.RequestUri = newUri;

            var response = await base.SendAsync(request, cancellationToken);

            if (ResponseIsValid(response))
            {
                StringBuilder nwContent = await TransformToSData(response);

                if (nwContent.Length > 0)
                {
                    string payload = nwContent.ToString();
                    if (acceptHeader.Any(x => x.Parameters.Any(p => p.Value == SDATA_MEDIA_TYPE_VALUE_FORMATTED)))
                    {
                        // only format payload (add line breaks, etc.) if user asks for it with header
                        payload = new JsonFormatter(payload).Format();
                    }

                    response.Content = new StringContent(payload);

                }
            }

            return response;
        }

        private static async Task<StringBuilder> TransformToSData(HttpResponseMessage response)
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
            else if (responseObject is IEnumerable)
            {
                nwContent = nwContent.Replace("\"Items\":[", "\"$resources\":[");
            }
            else if (origContent.Contains(ODATA_ELEMENT_KEY))
            {
                int posmetaend = origContent.IndexOf(ODATA_ELEMENT_KEY);
                nwContent = nwContent.Remove(0, posmetaend + ODATA_ELEMENT_KEY.Length);
                nwContent = nwContent.Insert(0, "{\n");
            }

            nwContent = nwContent.Replace("__SDataMetadata__", "$");
            nwContent = nwContent.Replace("odata.metadata", "$metadata");

            return nwContent;
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