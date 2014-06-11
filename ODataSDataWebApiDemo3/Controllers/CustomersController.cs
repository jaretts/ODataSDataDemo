using DemoModel;
using DemoModel.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace ODataSDataWebApiDemo3.Controllers
{
    public class CustomersController : ODataController
    {
        NephosEntities dbContext = new NephosEntities();

        //[Queryable]
        // Queryable single entity (i.e. include support) requires naming convention so declare
        // method Get[+ EntityName] like below and call helper method GetEntity in base
        public SingleResult<Customer> GetCustomer(string key)
        {
            Guid custKey = KeyToGuid(key);

            IQueryable<Customer> custs = dbContext.Customers.Where(c => c.Id == custKey);

            SingleResult<Customer> retVal = SingleResult.Create(custs);

            return retVal;
        }

        //[Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]//(AllowedQueryOptions = AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Expand | AllowedQueryOptions.Select | AllowedQueryOptions.InlineCount)]
        public PageResult<Customer> Get(ODataQueryOptions<Customer> options)
        {
            Guid VALID_TENANT_ID = Guid.Parse("7b6791d5-8658-44e5-86bc-8181735d0bf7");

            IQueryable query = dbContext.Customers.Where(c => c.Tenant_Id == VALID_TENANT_ID);

            ODataQuerySettings odataSettings = new ODataQuerySettings();

            if (options.Filter != null)
            {
                // set validator that restricts client's filtering 
                options.Filter.Validator = new QuoteQueryValidator();
                options.Validate(new ODataValidationSettings());

                query = options.Filter.ApplyTo(query, odataSettings);
            }

            // get the total count
            IQueryable<Customer> custQuery = query as IQueryable<Customer>;
            long totalCount = custQuery.LongCount();

            if (options.OrderBy != null)
            {
                query = options.OrderBy.ApplyTo(query, odataSettings);
            }

            if (options.Skip != null)
                query = options.Skip.ApplyTo(query, odataSettings);

            if (options.Top != null)
                query = options.Top.ApplyTo(query, odataSettings);


            //if (options.SelectExpand != null)
              //  query = options.SelectExpand.ApplyTo(query, odataSettings);

            if (options.SelectExpand != null)
                Request.SetSelectExpandClause(options.SelectExpand.SelectExpandClause);

            var retValue = new PageResult<Customer>(
                    query as IEnumerable<Customer>,
                    Request.GetNextPageLink(),
                    totalCount);

            return retValue;
        }


        private Guid KeyToGuid(string key)
        {
            key = key.Trim();

            if (key.StartsWith("\'"))
            {
                key = key.Substring(1);
            }

            if (key.EndsWith("\'"))
            {
                key = key.Remove(key.LastIndexOf("\'"));
            }

            return Guid.Parse(key);
        }

    }

    /*
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
     */
}
