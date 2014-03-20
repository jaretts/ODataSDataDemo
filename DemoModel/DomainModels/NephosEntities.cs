using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sage.SData.Entity;

namespace DemoModel.Model
{
    public abstract class SDataBaseEntity : SDataEntity
    {
        // Property name aliasing not supported but coming
        // see: https://aspnetwebstack.codeplex.com/discussions/462757
        //[DataMember(Name = "$descriptor")]
        //public string descriptor { get; set; }
        public string __SDataMetadata__descriptor { get; set; }
        public string __SDataMetadata__key { get; set; }
        public Nullable<System.DateTimeOffset> __SDataMetadata__updated { get; set; }

        abstract public void SetKey(System.Guid idValue);
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
    }

    public partial class Customer : SDataBaseEntity
    {
        public override void SetKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Tenant : SDataBaseEntity
    {
        public override void SetKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Order : SDataBaseEntity
    {
        public override void SetKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class QuoteDetail : SDataBaseEntity
    {
        public override void SetKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }
    public partial class InventoryItem : SDataBaseEntity
    {
        public override void SetKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

}