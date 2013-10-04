using DemoModel.Model;
using Microsoft.Data.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;

namespace ODataSDataWebApiDemo3
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapODataRoute("Nephos", "sdata", GetImplicitEDM());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            var queryAttribute = new QueryableAttribute()
            {
                AllowedQueryOptions = AllowedQueryOptions.All, // or just some: AllowedQueryOptions.Top | AllowedQueryOptions.Skip,
                MaxTop = 100,
                PageSize = 100,
            };
            config.EnableQuerySupport(queryAttribute);
            // no argument also allowed and then specify queryable attribute on controller
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }

        private static IEdmModel GetImplicitEDM()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            var selfAction = builder.Entity<Customer>().TransientAction("self");
            selfAction.HasActionLink(ctx =>
            {
                // or simply: return ctx.Url.Request.RequestUri
                var cust = ctx.EntityInstance as Customer;
                if (cust.EntityStatus == cust.EntityStatus)
                {
                    return new Uri(ctx.Url.ODataLink(
                        new EntitySetPathSegment(ctx.EntitySet),
                        new KeyValuePathSegment(cust.Id.ToString()))); //,
                        //new ActionPathSegment(checkoutAction.Name)));
                }
                else
                {
                    return null;
                }
            }, followsConventions: false);

            var editAction = builder.Entity<Customer>().TransientAction("edit");
            editAction.HasActionLink(ctx =>
            {
                var cust = ctx.EntityInstance as Customer;
                return new Uri(ctx.Url.ODataLink(new EntitySetPathSegment(ctx.EntitySet),
                    new KeyValuePathSegment(cust.Id.ToString())));
            } , followsConventions: false);


            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<ClickToPayPayment>("ClickToPayPayments");
            builder.EntitySet<Contact>("Contacts");
            builder.EntitySet<CustomerPortalSetting>("CustomerPortalSettings");
            builder.EntitySet<CustomerPrimary>("CustomerPrimarys");
            builder.EntitySet<DocumentTemplate>("DocumentTemplates");
            builder.EntitySet<EmailHistory>("EmailHistories");
            builder.EntitySet<EmailTemplate>("EmailTemplates");
            builder.EntitySet<IntegrationError>("IntegrationErrors");
            builder.EntitySet<InventoryCategory>("InventoryCategories");
            builder.EntitySet<InventoryItemBlob>("InventoryItemBlobs");
            builder.EntitySet<InventoryItem>("InventoryItems");
            builder.EntitySet<InvoiceDetail>("InvoiceDetails");
            builder.EntitySet<InvoicePayment>("InvoicePayments");
            builder.EntitySet<Invoice>("Invoices");
            builder.EntitySet<OrderDetail>("OrderDetails");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Payment>("Payments");
            builder.EntitySet<QuoteDetail>("QuoteDetails");

            //builder.EntitySet<QuoteSyncDetail>("QuoteSyncDetails");
            
            builder.EntitySet<Quote>("Quotes");

            //not found ? builder.EntitySet<RelatedInventoryItem>("RelatedInventoryItems");

            builder.EntitySet<SalesTeamMemberGoalPeriod>("SalesTeamMemberGoalPeriods");
            builder.EntitySet<SalesTeamMember>("SalesTeamMembers");
            builder.EntitySet<ServicesUser>("ServicesUsers");
            builder.EntitySet<ServiceType>("ServiceTypes");
            builder.EntitySet<SyncDigest>("SyncDigests");
            builder.EntitySet<Tenant>("Tenants");
            builder.EntitySet<UnprocessedInvoice>("UnprocessedInvoices");
            builder.EntitySet<User>("Users");
            builder.EntitySet<Version>("Versions");
            //builder.EntitySet<VersionInfo>("VersionInfos");
            builder.EntitySet<WorkOrderAttachment>("WorkOrderAttachments");
            builder.EntitySet<WorkOrderDetail>("WorkOrderDetails");
            builder.EntitySet<WorkOrder>("WorkOrders");
            return builder.GetEdmModel();
        }

    }
}
