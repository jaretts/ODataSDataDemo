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
    
    public partial class Order
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public int Status { get; set; }
        public int OrderType { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> SandH { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal SubTotal { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public string ExternalReference { get; set; }
        public string External_Id { get; set; }
        public System.Guid Customer_Id { get; set; }
        public System.Guid User_Id { get; set; }
        public System.Guid ShippingAddress_Id { get; set; }
        public Nullable<decimal> DiscountPercent { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
        public string AuthorizationCode { get; set; }
        public string CreditCardLast4 { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string SyncHashKey { get; set; }
        public short SyncEndpoint_Id { get; set; }
        public int SyncEndpointTick { get; set; }
        public short EntityStatus { get; set; }
        public string TaxTransactionId { get; set; }
    }
}
