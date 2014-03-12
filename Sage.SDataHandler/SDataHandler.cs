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
using Sage.SData.Entity;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using Sage.SDataHandler.ContentTypes;

namespace Sage.SDataHandler
{
    public class SDataHandler : DelegatingHandler //AuthenticationHandler
    {
        private const string ODATA_ELEMENT_KEY = "@Element\",";
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string ALL_MEDIA_TYPE = "*/*";
        private const string SDATA_MEDIA_TYPE_PARAM_VND = "vnd.sage";
        private const string SDATA_MEDIA_TYPE_VALUE_VND = "sdata";
        private const string ODATA_MEDIA_TYPE_VALUE_VND = "odata";
        private const string ODATA_MEDIA_TYPE_PARAM_VND = "odata";
        private const string SDATA_MEDIA_TYPE_VALUE_FORMATTED = "formatted";

        private string _targetRoutPrefix;

        public string TargetRoutPrefix
        {
            get { return _targetRoutPrefix; }
            set 
            {
                _targetRoutPrefix = value;

                if (!String.IsNullOrEmpty(_targetRoutPrefix))
                {
                    _targetRoutPrefix = _targetRoutPrefix.Trim();

                    if(!_targetRoutPrefix.StartsWith("/"))
                        _targetRoutPrefix = "/" + _targetRoutPrefix;

                }
            }
        }

        public SDataHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }

        // if targetRoutPrefix is not empty or null then only URLs starting 
        // with that prefix will be mapped to OData
        public SDataHandler(string init_targetRoutPrefix)
        {
            TargetRoutPrefix = init_targetRoutPrefix;
        }

        public SDataHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var acceptHeader = request.Headers.Accept;

            bool acceptsAll = acceptHeader.Any(x => x.MediaType == ALL_MEDIA_TYPE);

            if (!acceptsAll)
            {
                bool acceptsJson = acceptHeader.Any(x => x.MediaType == JSON_MEDIA_TYPE);

                if (!acceptsJson)
                {
                    return await base.SendAsync(request, cancellationToken);
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
                        return await base.SendAsync(request, cancellationToken);
                    }

                    // consumer asked for JSON but did not indicated SData or OData so default is return SData; just continue
                }

            }
            Uri originalUri = request.RequestUri;

            if (TargetRoutPrefix != null && TargetRoutPrefix.Trim() != ""
                && !originalUri.AbsolutePath.StartsWith(TargetRoutPrefix))
            {
                // there's a targeRoutePrefix specified so skip mapping if this
                // URL doesn't start with the prefix
                return await base.SendAsync(request, cancellationToken);
            }

            // check if a param like format=json was in original uri and set Accept header
            //SDataUriUtil.SetAcceptJsonHeader(request, originalUri);

            // convert any SData query keys (where, startIndex, etc.) 
            Uri newUri = SDataUriUtil.TranslateUri(originalUri, SDataUriKeys.CONVERT_TO_ODATA);
            request.RequestUri = newUri;

            // replace consumers Accept Header with OData nometadata so we get json in format we want
            request.Headers.Accept.Clear();
            MediaTypeWithQualityHeaderValue noMetadataHeader = new MediaTypeWithQualityHeaderValue("application/json");
            noMetadataHeader.Parameters.Add( new NameValueWithParametersHeaderValue("odata","nometadata") );
            request.Headers.Accept.Add(noMetadataHeader);

            var response = await base.SendAsync(request, cancellationToken);

            if (ResponseIsValid(response))
            {
                response.Content = new SDataContent(response);
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