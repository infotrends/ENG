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
    
    public partial class new_OfficeTemplate
    {
        public new_OfficeTemplate()
        {
            this.new_OfficeTemplateVersion = new HashSet<new_OfficeTemplateVersion>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> MarkDeleteOn { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual ICollection<new_OfficeTemplateVersion> new_OfficeTemplateVersion { get; set; }
    }
}
