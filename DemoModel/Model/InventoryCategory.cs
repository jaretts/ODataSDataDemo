//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoModel.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventoryCategory
    {
        public InventoryCategory()
        {
            this.InventoryCategories1 = new HashSet<InventoryCategory>();
            this.InventoryItems = new HashSet<InventoryItem>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public Nullable<System.Guid> Parent_Id { get; set; }
    
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<InventoryCategory> InventoryCategories1 { get; set; }
        public virtual InventoryCategory InventoryCategory1 { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
