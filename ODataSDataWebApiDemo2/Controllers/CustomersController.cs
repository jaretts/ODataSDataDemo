using ODataSDataWebApiDemo2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace ODataSDataWebApiDemo2.Controllers
{
    public class CustomersController : DynamicController<Customer, string>
    {
        public IQueryable<Tenant> GetTenantsFromCustomer([FromODataUri] string key)
        {
            Guid custKey = Guid.Parse(key);

            IQueryable<Tenant> tens = GetDatabaseContext().Set<Tenant>().Where(t => t.Customers.Any(c => c.Id == custKey));

            return tens;
        }
    }
}
