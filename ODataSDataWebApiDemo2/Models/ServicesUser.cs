//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ODataSDataWebApiDemo2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServicesUser
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public System.Guid User_Id { get; set; }
        public int Role { get; set; }
        public string Notes { get; set; }
    }
}
