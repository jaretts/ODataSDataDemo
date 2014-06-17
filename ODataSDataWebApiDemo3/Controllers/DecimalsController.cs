using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace ODataSDataWebApiDemo3.Controllers 
{
    public class DecimalsController : ODataController
    {
        public SingleResult<DecimalDemo> GetDecimalDemo(string key)
        {
            List<DecimalDemo> retList = new List<DecimalDemo>() { new DecimalDemo() };

            SingleResult<DecimalDemo> retVal = SingleResult.Create(retList.AsQueryable<DecimalDemo>());

            return retVal;
        }

        public PageResult<DecimalDemo> Get(ODataQueryOptions<DecimalDemo> options)
        {
            List<DecimalDemo> retList = new List<DecimalDemo>() { new DecimalDemo() };

            return new PageResult<DecimalDemo>(
                    retList.AsEnumerable<DecimalDemo>(),
                    null,
                    1);
        }

    }

    public class DecimalDemo 
    {
        decimal _price = 123.12M;
        string _name = "PC";

        public decimal Price 
        {
            get { return _price; }
            set { _price = value; } 
        }

        public string ProductName
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}