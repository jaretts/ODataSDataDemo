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
    
    public partial class OrderDetail
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public System.Guid Order_Id { get; set; }
        public Nullable<System.Guid> InventoryItem_Id { get; set; }
        public string External_Id { get; set; }
        public string ExternalReference { get; set; }
        public string SyncHashKey { get; set; }
        public short SyncEndpoint_Id { get; set; }
        public int SyncEndpointTick { get; set; }
        public short EntityStatus { get; set; }
    }
}