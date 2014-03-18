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
using System.Collections;
using Sage.SData.Entity;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using Sage.SDataHandler.ContentTypes;
using Sage.SDataHandler.Uris;

namespace Sage.SDataHandler
{
    public class SDataMessageHandler : DelegatingHandler
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string ALL_MEDIA_TYPE = "*/*";
        private const string SDATA_MEDIA_TYPE_PARAM_VND = "vnd.sage";
        private const string SDATA_MEDIA_TYPE_VALUE_VND = "sdata";
        private const string ODATA_MEDIA_TYPE_VALUE_VND = "odata";
        private const string ODATA_MEDIA_TYPE_PARAM_VND = "odata";

        private string _targetRoutPrefix;

        public SDataMessageHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }

        /// <summary>
        /// Creates Message Handler that only converts requests/Url starting with the 
        /// targetRoutePrefix
        /// </summary>
        /// <param name="init_targetRoutPrefix">
        /// if targetRoutPrefix is not empty or null then only URLs starting with that prefix
        /// are mapped to OData; all other requests just pass through as is
        /// </param>
        public SDataMessageHandler(string init_targetRoutPrefix)
        {
            TargetRoutPrefix = init_targetRoutPrefix;
        }

        public SDataMessageHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(!ValidateAcceptsSData(request.Headers.Accept))
            {
                // consumer does NOT want SData so let Web API handle this as is
                return await base.SendAsync(request, cancellationToken);
            }

            Uri originalUri = request.RequestUri;

            // check if this server only specified some routes mapped to SData
            if (TargetRoutPrefix != null && TargetRoutPrefix.Trim() != ""
                && !originalUri.AbsolutePath.StartsWith(TargetRoutPrefix))
            {
                // there's a targeRoutePrefix specified so skip mapping if this
                // URL doesn't start with the prefix
                return await base.SendAsync(request, cancellationToken);
            }

            // convert SData query keys (where, startIndex, etc.) 
            Uri newUri = SDataUriUtil.TranslateUri(originalUri, SDataUriKeys.CONVERT_TO_ODATA);
            request.RequestUri = newUri;

            // replace consumer's Accept Header with OData nometadata so we get json in format we want
            request.Headers.Accept.Clear();
            MediaTypeWithQualityHeaderValue noMetadataHeader = new MediaTypeWithQualityHeaderValue(JSON_MEDIA_TYPE);
            noMetadataHeader.Parameters.Add( new NameValueWithParametersHeaderValue("odata","nometadata") );
            request.Headers.Accept.Add(noMetadataHeader);

            // URL and Headers have been mapped to OData; we now have an OData request 
            var response = await base.SendAsync(request, cancellationToken);

            if (ResponseIsValid(response))
            {
                // OData request was handled and is valid so now transform Content/Payload to SData
                response.Content = new SDataContent(response);
            }

            return response;
        }

        private bool ValidateAcceptsSData(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeader)
        {
            if (acceptHeader != null && !acceptHeader.Any(x => x.MediaType == ALL_MEDIA_TYPE))
            {
                bool acceptsJson = acceptHeader.Any(x => x.MediaType == JSON_MEDIA_TYPE);

                if (!acceptsJson)
                {
                    return false;
                }

                bool acceptsSData = acceptHeader.Any(x => x.MediaType == JSON_MEDIA_TYPE &&
                                    x.Parameters.Any(p => p.Name == SDATA_MEDIA_TYPE_VALUE_VND));

                if (!acceptsSData)
                {
                    // does not mention SData check if OData mentioned
                    bool acceptsOData = acceptHeader.Any(x => x.MediaType == JSON_MEDIA_TYPE &&
                                        x.Parameters.Any(p => p.Name == ODATA_MEDIA_TYPE_VALUE_VND));

                    if (acceptsOData)
                    {
                        // OData JSON Request; i.e. Accept header equals: application/json;odata=[verbose, etc]
                        return false;
                    }

                    // consumer asked for JSON but did not indicated SData or OData so default is return SData; just continue
                }
            }

            return true;
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

        private string TargetRoutPrefix
        {
            get { return _targetRoutPrefix; }
            set
            {
                _targetRoutPrefix = value;

                if (!String.IsNullOrEmpty(_targetRoutPrefix))
                {
                    _targetRoutPrefix = _targetRoutPrefix.Trim();

                    if (!_targetRoutPrefix.StartsWith("/"))
                        _targetRoutPrefix = "/" + _targetRoutPrefix;

                }
            }
        }

    }

}