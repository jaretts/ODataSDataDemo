using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoModel.Model
{
    public partial class Customer : NephosEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Tenant : NephosEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

    public partial class Order : NephosEntity
    {
        public override void SetNephosKey(System.Guid idValue)
        {
            Id = idValue;
        }
    }

}