using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sage.SData.Entity
{
    abstract public class SDataEntity
    {
        // Property name aliasing not supported but coming
        // see: https://aspnetwebstack.codeplex.com/discussions/462757
        //[DataMember(Name = "$descriptor")]
        //public string descriptor { get; set; }
        public string __SDataMetadata__descriptor { get; set; }

        public string __SDataMetadata__key { get; set; }

        public Nullable<System.DateTimeOffset> __SDataMetadata__updated { get; set; }
    }
}