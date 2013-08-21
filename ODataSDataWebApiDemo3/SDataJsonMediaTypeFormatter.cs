using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System;
using System.Net.Http;
using Microsoft.Data.OData;
using System.Collections.Generic;
using System.Web.Http.OData.Formatter.Serialization;

namespace ODataSDataWebApiDemo3
{
    /// <summary>
    /// <see cref="MediaTypeFormatter"/> class to handle Json.
        /*
    /// </summary>
    public class SDataJsonMediaTypeFormatter : System.Web.Http.OData.Formatter.Serialization.DefaultODataSerializerProvider //System.Web.Http.OData.Formatter.ODataMediaTypeFormatter // JsonMediaTypeFormatter
    {
        public SDataJsonMediaTypeFormatter(IEnumerable<ODataPayloadKind> payloadKinds) : base(payloadKinds)
        {
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }
    }
         */
}
