using DemoModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Sage.SData.Entity;
using System.Web.Http.OData.Query.Validators;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query.SemanticAst;

namespace ODataSDataWebApiDemo3.Controllers
{
    abstract public class DynamicController<TEntity,TKey> : System.Web.Http.OData.EntitySetController<TEntity, string>
        where TEntity : SDataBaseEntity
    {

        NephosEntities dbContext;
        Guid VALID_TENANT_ID = Guid.Parse("7b6791d5-8658-44e5-86bc-8181735d0bf7");

        protected DbContext GetDatabaseContext()
        {
            if (dbContext == null)
            {
                dbContext = new NephosEntities();
            }

            return dbContext;
        }

        /* This method relies OData filtering, etc., to happen higher in the stack and not in our code
        //[Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IEnumerable<TEntity> Get()
        {
            return GetDatabaseContext().Set<TEntity>().Where( e => e.Tenant_Id == VALID_TENANT_ID);
        }
        */

        // this method illustrates 
        public override IQueryable<TEntity> Get()
        {
            ODataQueryOptions<TEntity> options = QueryOptions;

            IQueryable results = GetDatabaseContext()
                            .Set<TEntity>().Where( e => e.Tenant_Id == VALID_TENANT_ID);

            var validationSettings = new ODataValidationSettings()
            {
                // Initialize settings as needed.
                MaxTop = 10
            };
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = 10,
            };

            if (options.Filter != null)
            {
                // set validator that restricts client's filtering 
                options.Filter.Validator = new QuoteQueryValidator();
                options.Validate(validationSettings);

                results = options.Filter.ApplyTo(results, settings);
            }

            return results.Cast<TEntity>();
        }

        protected override TEntity GetEntityByKey(string key)
        {
            Guid custKey = Guid.Parse(key);

            return GetDatabaseContext().Set<TEntity>().FirstOrDefault(c => c.Id == custKey);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            GetDatabaseContext().Dispose();
        }


        protected override TEntity PatchEntity([FromODataUri] string key, Delta<TEntity> patch)
        {
            Guid gKey = Guid.Parse(key);
            var entity = GetDatabaseContext().Set<TEntity>().FirstOrDefault(c => c.Id == gKey);
            if (entity == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            patch.Patch(entity);
            GetDatabaseContext().SaveChanges();

            return entity;
        }

        protected override TEntity UpdateEntity([FromODataUri] string key, TEntity update)
        {
            update.SetKey(Guid.Parse(key));
            GetDatabaseContext().Set<TEntity>().Attach(update);
            GetDatabaseContext().Entry(update).State = System.Data.Entity.EntityState.Modified;
            GetDatabaseContext().SaveChanges();

            return update;
        }

        protected override TEntity CreateEntity(TEntity entity)
        {
            Guid newId = Guid.NewGuid();
            entity.SetKey(newId);
            GetDatabaseContext().Set<TEntity>().Add(entity);
            GetDatabaseContext().SaveChanges();
            return entity;
        }

        protected override string GetKey(TEntity entity)
        {
            return entity.Id.ToString();
        }

        public override void Delete([FromODataUri] string key)
        {
            Guid gKey = Guid.Parse(key);
            var entity = GetDatabaseContext().Set<TEntity>().FirstOrDefault(c => c.Id == gKey);
            GetDatabaseContext().Set<TEntity>().Remove(entity);
            GetDatabaseContext().SaveChanges();

            return;
        }
    }

    public class QuoteQueryValidator : FilterQueryValidator
    {
        List<string> restrictedProperties = new List<string>() { "Tenant_Id" };

        public override void ValidateSingleValuePropertyAccessNode(SingleValuePropertyAccessNode propertyAccessNode, ODataValidationSettings settings)
        {

            if (restrictedProperties.Contains<string>(propertyAccessNode.Property.Name))
            {
                throw new ODataException(string.Format("{0} is an invalid filter property"));
            }
            base.ValidateSingleValuePropertyAccessNode(propertyAccessNode, settings);
        }
    }

}
