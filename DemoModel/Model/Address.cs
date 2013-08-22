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
    
    public partial class Address
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string Street4 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public Nullable<System.Guid> Contact_Id { get; set; }
        public Nullable<System.Guid> Customer_Id { get; set; }
        public string External_Id { get; set; }
        public string Country { get; set; }
        public string ExternalReference { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string SyncHashKey { get; set; }
        public short SyncEndpoint_Id { get; set; }
        public int SyncEndpointTick { get; set; }
        public short EntityStatus { get; set; }
    }
}