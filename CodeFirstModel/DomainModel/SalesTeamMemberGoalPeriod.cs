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
    
    public partial class SalesTeamMemberGoalPeriod
    {
        public System.Guid Id { get; set; }
        public System.Guid Tenant_Id { get; set; }
        public System.DateTime GoalStartDate { get; set; }
        public System.DateTime GoalEndDate { get; set; }
        public decimal GoalAmount { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public Nullable<System.Guid> SalesTeamMember_Id { get; set; }
    }
}
