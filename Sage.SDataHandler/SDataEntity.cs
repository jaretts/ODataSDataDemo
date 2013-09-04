using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sage.SData.Entity
{
    abstract public class SDataEntity
    {
        abstract public void SetNephosKey(System.Guid idValue);
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
    }
}