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

        [Queryable]
        // Queryable single entity (i.e. include support) requires naming convention so declare
        // method Get[+ EntityName] like below and call helper method GetEntity in base
        public SingleResult<Customer> GetCustomer(string key)
        {
            return base.GetEntity(key);
        }

        public IQueryable<Tenant> GetTenantFromCustomer([FromODataUri] string key)
        {
            Guid custKey = Guid.Parse(key);

            IQueryable<Tenant> tens = GetDatabaseContext().Set<Tenant>().Where(t => t.Customers.Any(c => c.Id == custKey));

            return tens;
        }

        public IQueryable<Order> GetOrdersFromCustomer([FromODataUri] string key)
        {
            Guid custKey = Guid.Parse(key);

            IQueryable<Order> tens = GetDatabaseContext().Set<Order>().Where(t => t.Customer_Id == custKey);

            return tens;
        }
    }
}
