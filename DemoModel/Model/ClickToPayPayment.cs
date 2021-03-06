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
    
    public partial class ClickToPayPayment
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public decimal TotalPaid { get; set; }
        public bool Paid { get; set; }
        public Nullable<System.DateTime> DatePaid { get; set; }
        public string ResponseMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public Nullable<System.Guid> InvoiceEmailHistory_Id { get; set; }
        public System.Guid Transaction_Id { get; set; }
        public bool WebsiteAccessed { get; set; }
        public bool PaymentAttempted { get; set; }
        public bool PaymentAttemptFailed { get; set; }
        public Nullable<System.Guid> Invoice_Id { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string ResponseIndicator { get; set; }
        public string ResponseCode { get; set; }
        public string VANReference { get; set; }
    }
}
