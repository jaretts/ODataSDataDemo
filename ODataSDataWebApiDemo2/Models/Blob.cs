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
    
    public partial class Blob
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> BlobName { get; set; }
        public string MimeType { get; set; }
        public string Url { get; set; }
        public Nullable<System.Guid> ParentEntityId { get; set; }
        public int Type { get; set; }
        public int App { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
    
        public virtual Tenant Tenant { get; set; }
    }
}
