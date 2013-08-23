using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DemoModel.Model
{
    abstract public class NephosEntity
    {
        abstract public void SetNephosKey(System.Guid idValue);
        public System.Guid Id { get; set; }
    }
}