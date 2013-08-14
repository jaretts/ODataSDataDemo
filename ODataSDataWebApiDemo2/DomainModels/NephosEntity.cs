using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ODataSDataWebApiDemo2.Models
{
    public class NephosEntity
    {
        [DataMember(Name = "$key")]
        public virtual Guid Id { get { return Guid.Empty; } }
    }
}