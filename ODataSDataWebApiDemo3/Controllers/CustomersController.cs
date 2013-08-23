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
    public class CustomersController : DynamicController<Customer, string>
    {
        public IQueryable<Tenant> GetTenantFromCustomer([FromODataUri] string key)
        {
            Guid custKey = Guid.Parse(key);

            IQueryable<Tenant> tens = GetDatabaseContext().Set<Tenant>().Where(t => t.Customers.Any(c => c.Id == custKey));

            return tens;
        }

        public IQueryable<Quote> GetQuoteFromCustomer([FromODataUri] string key)
        {
            Guid custKey = Guid.Parse(key);

            IQueryable<Quote> tens = GetDatabaseContext().Set<Quote>().Where(t => t.Customer_Id == custKey);

            return tens;
        }

    }
}
