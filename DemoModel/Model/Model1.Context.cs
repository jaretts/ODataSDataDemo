﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NephosEntities : DbContext
    {
        public NephosEntities()
            : base("name=NephosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<ClickToPayPayment> ClickToPayPayments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CustomerPortalSetting> CustomerPortalSettings { get; set; }
        public DbSet<CustomerPrimary> CustomerPrimaries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<EmailHistory> EmailHistories { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<IntegrationError> IntegrationErrors { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }
        public DbSet<InventoryItemBlob> InventoryItemBlobs { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<QuoteDetail> QuoteDetails { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<SalesTeamMemberGoalPeriod> SalesTeamMemberGoalPeriods { get; set; }
        public DbSet<SalesTeamMember> SalesTeamMembers { get; set; }
        public DbSet<ServicesUser> ServicesUsers { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<SyncDigest> SyncDigests { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UnprocessedInvoice> UnprocessedInvoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VersionInfo> VersionInfoes { get; set; }
        public DbSet<WorkOrderAttachment> WorkOrderAttachments { get; set; }
        public DbSet<WorkOrderDetail> WorkOrderDetails { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
    }
}