using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sage.SData.Entity;

namespace DemoModel.Model
{
    public partial class Customer : SDataEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Tenant : SDataEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Order : SDataEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

}