using ODataSDataWebApiDemo2.Models;
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
    abstract public class DynamicController<TEntity,TKey> : System.Web.Http.OData.EntitySetController<TEntity, string>
        where TEntity : NephosEntity
    {
        protected DbContext GetDatabaseContext()
        {
            return new NephosEntities();
        }

        [Queryable(MaxTop = 3, PageSize = 3)]
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
    }
}
