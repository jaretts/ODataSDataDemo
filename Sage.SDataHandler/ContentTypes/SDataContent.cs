using Sage.SData.Entity;
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

        private const string SDATA_METADATA_PREFIX = "__SDataMetadata__";

        private HttpContent originalContent;
        HttpResponseMessage origResponse;
        object responseObject;

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

                    string line;
                    bool collectionFound = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!collectionFound)
                        {
                            if (respContentType == CONTENT_ODATA_COLLECTION
                                || respContentType == CONTENT_API_COLLECTION)
                            {
                                // payload contains a collection
                                if (line.Contains(COLLECTION_NAME_ODATA))
                                {
                                    line = line.Replace(COLLECTION_NAME_ODATA, BuildPagingResponse() + COLLECTION_NAME_SDATA);
                                    line = line.Replace("odata.count", "$totalResults");

                                    collectionFound = true;
                                }
                                else if (line.Contains(COLLECTION_NAME_API))
                                {
                                    line = line.Replace(COLLECTION_NAME_API, BuildPagingResponse() + COLLECTION_NAME_SDATA);

                                    collectionFound = true;
                                }
                            }
                            else
                            {
                                // if payload does not contain a collection don't keep searching
                                collectionFound = true;
                            }
                        }

                        // this is required until property name aliasing is supported
                        // see: https://aspnetwebstack.codeplex.com/discussions/462757
                        if(line.Contains(SDATA_METADATA_PREFIX))
                            line = line.Replace(SDATA_METADATA_PREFIX, "$");

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
            int startIndex; // = SDataUriUtil.GetSDataStartIndexValue(response.RequestMessage.RequestUri);
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