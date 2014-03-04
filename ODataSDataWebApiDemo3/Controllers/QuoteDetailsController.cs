using DemoModel;
using DemoModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace ODataSDataWebApiDemo3.Controllers
{
    public class QuoteDetailsController : DynamicController<QuoteDetail, string>
    {
    }
}
