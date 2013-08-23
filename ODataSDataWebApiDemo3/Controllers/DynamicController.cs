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

namespace ODataSDataWebApiDemo3.Controllers
{
    abstract public class DynamicController<TEntity,TKey> : System.Web.Http.OData.EntitySetController<TEntity, string>
        where TEntity : DemoModel.Model.NephosEntity
    {

        NephosEntities dbContext;

        protected DbContext GetDatabaseContext()
        {
            if (dbContext == null)
            {
                dbContext = new NephosEntities();
            }

            return dbContext;
        }

        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<TEntity> Get()
        {
            return GetDatabaseContext().Set<TEntity>();
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
            update.SetNephosKey(Guid.Parse(key));
            GetDatabaseContext().Set<TEntity>().Attach(update);
            GetDatabaseContext().Entry(update).State = System.Data.EntityState.Modified;
            GetDatabaseContext().SaveChanges();

            return update;
        }

        protected override TEntity CreateEntity(TEntity entity)
        {
            Guid newId = Guid.NewGuid();
            entity.SetNephosKey(newId);
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
}
