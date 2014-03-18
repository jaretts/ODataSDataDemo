using Sage.SData.Entity;
using Sage.SDataHandler.Uris;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sage.SDataHandler.ContentTypes
{
    class SDataContent : HttpContent
    {
        private const int CONTENT_SINGLE_RESOURCE = 1;
        private const int CONTENT_ODATA_COLLECTION = 2;
        private const int CONTENT_API_COLLECTION = 3;

        private const string COLLECTION_NAME_ODATA  = "\"value\":[";
        private const string COLLECTION_NAME_API    = "\"Items\":[";
        private const string COLLECTION_NAME_SDATA = "\"$resources\":[";

        // Eliminate this as soon as support for property name Aliasing is supported
        private const string SDATA_METADATA_PROPNAME_PREFIX = "__SDataMetadata__";
        private const string SDATA_METADATA_ACTUAL_PREFIX = "$";

        private const string PARAM_TOTALRESULT_SDATA = "$totalResults";
        private const string PARAM_TOTALRESULT_ODATA = "odata.count";

        private HttpContent originalContent;
        HttpResponseMessage origResponse;
        object responseObject;

        /// <summary>
        /// SDataContent transforms OData/Web API Content/Response to SData
        /// </summary>
        /// <param name="response">The response containing the Content/Payload to transform</param>
        public SDataContent(HttpResponseMessage response)
        {
            origResponse = response;
            originalContent = origResponse.Content;

            if (originalContent == null)
            {
                throw new ArgumentNullException("content");
            }

            foreach (var header in originalContent.Headers)
            {
                this.Headers.Add(header.Key, header.Value);
            }

            origResponse.TryGetContentValue(out responseObject);

        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return TransformToSDataPayload(stream);
        }

        private async Task TransformToSDataPayload(Stream targetStream)
        {
            //await oContentStream.CopyToAsync(targetStream);
            using (var oContentStream = await originalContent.ReadAsStreamAsync())
            {
                StreamReader reader = null;
                StreamWriter outStream = null;
                try
                {
                    reader = new StreamReader(oContentStream);
                    outStream = new StreamWriter(targetStream);

                    int respContentType = GetContentType();
                    bool searchForCollection = respContentType == CONTENT_ODATA_COLLECTION
                                        || respContentType == CONTENT_API_COLLECTION;

                    string line; 
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (searchForCollection)
                        {
                            // payload contains a collection so convert the array's property name 
                            // from "Item" or "Value" to SData's "$resource". This should be found 
                            // in second line so after this stop scanning and just copy the rest of the payload
                            if (line.Contains(COLLECTION_NAME_ODATA))
                            {
                                line = line.Replace(COLLECTION_NAME_ODATA, BuildPagingResponse() + COLLECTION_NAME_SDATA);
                                line = line.Replace(PARAM_TOTALRESULT_ODATA, PARAM_TOTALRESULT_SDATA);

                                searchForCollection = false;
                            }
                            else if (line.Contains(COLLECTION_NAME_API))
                            {
                                line = line.Replace(COLLECTION_NAME_API, BuildPagingResponse() + COLLECTION_NAME_SDATA);

                                searchForCollection = false;
                            }
                        }

                        // this is required until property name aliasing is supported by WebAPI/OData serializer
                        // see: https://aspnetwebstack.codeplex.com/discussions/462757
                        if(line.Contains(SDATA_METADATA_PROPNAME_PREFIX))
                            line = line.Replace(SDATA_METADATA_PROPNAME_PREFIX, SDATA_METADATA_ACTUAL_PREFIX);

                        outStream.WriteLine(line);
                    }

                    outStream.Flush();
                }
                catch (Exception exc)
                {
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    if (outStream != null)
                        outStream.Close();

                }
            }
        }

        private int GetContentType()
        {
            int retVal = CONTENT_SINGLE_RESOURCE;
            if (responseObject is IQueryable)
            {
                retVal = CONTENT_ODATA_COLLECTION;
            }
            else if (responseObject is IEnumerable)
            {
                retVal = CONTENT_API_COLLECTION;
            }

            return retVal;
        }

        private string BuildPagingResponse()
        {
            int startIndex;
            NameValueCollection query = origResponse.RequestMessage.RequestUri.ParseQueryString();
            if (query.AllKeys.Contains(SDataUriKeys.SDATA_STARTINDEX))
            {
                startIndex = int.Parse(query[SDataUriKeys.SDATA_STARTINDEX]);
            }
            else if (query.AllKeys.Contains(SDataUriKeys.ODATA_STARTINDEX))
            {
                startIndex = int.Parse(query[SDataUriKeys.ODATA_STARTINDEX]) + 1;
            }
            else
            {
                startIndex = 1;
            }

            int count = 0;
            if (responseObject is IQueryable<SDataEntity>)
            {
                // whenever possible use queriable count; better performance
                IQueryable<SDataEntity> q = (responseObject as IQueryable<SDataEntity>);
                count = q.Count<SDataEntity>();
            }
            else if (responseObject is IEnumerable)
            {
                var enumer = (responseObject as IEnumerable).GetEnumerator();
                while (enumer.MoveNext())
                {
                    count++;
                }
            }

            string pagingInfo = "\"$startIndex\":" + startIndex + ", \"$itemsPerPage\":" + count + ",";
            return pagingInfo;
        }

    }
}