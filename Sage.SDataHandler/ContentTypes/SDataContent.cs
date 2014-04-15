using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sage.SDataHandler.Uris;
using Sage.SData.Entity;


namespace Sage.SDataHandler.ContentTypes
{
    class SDataContent : HttpContent
    {
        private const int ContentSingleResource = 1;
        private const int ContentOdataCollection = 2;
        private const int ContentApiCollection = 3;

        private const string ParamTotalresultSdata = "$totalResults";
        private const string ParamTotalresultOdata = "odata.count";
        private const string ParamCollectionNameOdata = "value";
        private const string ParamCollectionNameApi = "Items";
        private const string ParamCollectionNameSdata = "$resources";

        readonly List<IContentMapper> maps;

        private readonly HttpContent originalContent;
        readonly HttpResponseMessage origResponse;
        readonly object responseObject;

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
                throw new ArgumentNullException("response");
            }

            foreach (var header in originalContent.Headers)
            {
                Headers.Add(header.Key, header.Value);
            }

            origResponse.TryGetContentValue(out responseObject);
        }

        /// <summary>
        /// SDataContent transforms OData/Web API Content/Response to SData
        /// </summary>
        /// <param name="response">The response containing the Content/Payload to transform</param>
        /// <param name="initMaps"></param>
        public SDataContent(HttpResponseMessage response, List<IContentMapper> initMaps)
            : this(response)
        {
            if (initMaps != null && initMaps.Count > 0)
            {
                maps = initMaps;
            }
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
            using (var oContentStream = await originalContent.ReadAsStreamAsync())
            {
                JsonReader reader = null;
                JsonWriter outStream = null;
                int level = -1;
                try
                {
                    reader = new JsonTextReader(new StreamReader(oContentStream));
                    outStream = new JsonTextWriter(new StreamWriter(targetStream));

                    int respContentType = GetContentType();
                    bool searchForCollection = respContentType == ContentOdataCollection
                                               || respContentType == ContentApiCollection;

                    while (reader.Read())
                    {
                        if (searchForCollection && level == 0 && reader.TokenType == JsonToken.PropertyName)
                        {
                            if ((string)reader.Value == ParamCollectionNameApi || (string)reader.Value == ParamCollectionNameOdata)
                            {
                                WritePagingProperties(outStream);
                                outStream.WritePropertyName(ParamCollectionNameSdata);
                            }
                            else if ((string)reader.Value == ParamTotalresultOdata)
                                outStream.WritePropertyName(ParamTotalresultSdata);
                            else
                                outStream.WriteToken(reader, false);
                        }
                        else if (reader.TokenType == JsonToken.PropertyName)
                            outStream.WritePropertyName(CallOptionalMaps((string)reader.Value));
                        else
                        {
                            if (reader.TokenType == JsonToken.StartObject)
                                level++;
                            else if (reader.TokenType == JsonToken.EndObject)
                                level--;
                            outStream.WriteToken(reader, false);
                        }
                    }
                    outStream.Flush();
                }
                catch (Exception e)
                {
                    // TODO can we get the user and tenant context from here?
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

        private string CallOptionalMaps(string line)
        {
            if (maps != null)
            {
                line = maps.Aggregate(line, (current, map) => map.Map(current));
            }

            return line;
        }

        private int GetContentType()
        {
            int retVal = ContentSingleResource;
            if (responseObject is IQueryable)
            {
                retVal = ContentOdataCollection;
            }
            else if (responseObject is IEnumerable)
            {
                retVal = ContentApiCollection;
            }

            return retVal;
        }


        private void WritePagingProperties(JsonWriter writer)
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
                var q = (responseObject as IQueryable<SDataEntity>);
                count = q.Count();
            }
            else if (responseObject is IEnumerable)
            {
                var enumer = (responseObject as IEnumerable).GetEnumerator();
                while (enumer.MoveNext())
                {
                    count++;
                }
            }
            writer.WritePropertyName("$startIndex");
            writer.WriteValue(startIndex);
            writer.WritePropertyName("$itemsPerPage");
            writer.WriteValue(count);
        }

    }
}