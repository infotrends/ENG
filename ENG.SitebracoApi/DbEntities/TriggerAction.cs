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
    
    public partial class TriggerAction
    {
        public int TriggerId { get; set; }
        public int ActionId { get; set; }
        public int SortOrder { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual Trigger Trigger { get; set; }
    }
}
