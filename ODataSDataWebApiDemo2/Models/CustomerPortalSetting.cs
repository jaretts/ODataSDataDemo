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
    
    public partial class CustomerPortalSetting
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public int NameFont { get; set; }
        public int NameFontSize { get; set; }
        public string SupportContact { get; set; }
        public int SupportContactFont { get; set; }
        public int SupportContactFontSize { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
    
        public virtual Tenant Tenant { get; set; }
    }
}
