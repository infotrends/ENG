//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SitebracoApi.DbEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisitorTriggerRule
    {
        public int TriggerRuleId { get; set; }
        public int Hits { get; set; }
        public long VisitorId { get; set; }
    
        public virtual TriggerRule TriggerRule { get; set; }
        public virtual Visitor Visitor { get; set; }
    }
}
