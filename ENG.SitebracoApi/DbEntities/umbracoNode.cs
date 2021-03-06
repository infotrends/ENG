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
    
    public partial class umbracoNode
    {
        public umbracoNode()
        {
            this.cmsContents = new HashSet<cmsContent>();
            this.cmsContentTypes = new HashSet<cmsContentType>();
            this.cmsDataTypes = new HashSet<cmsDataType>();
            this.cmsDocuments = new HashSet<cmsDocument>();
            this.cmsDocumentTypes = new HashSet<cmsDocumentType>();
            this.cmsMemberTypes = new HashSet<cmsMemberType>();
            this.cmsPropertyDatas = new HashSet<cmsPropertyData>();
            this.cmsTasks = new HashSet<cmsTask>();
            this.cmsTemplates = new HashSet<cmsTemplate>();
            this.cmsTemplates1 = new HashSet<cmsTemplate>();
            this.umbracoDomains = new HashSet<umbracoDomain>();
            this.umbracoNode1 = new HashSet<umbracoNode>();
            this.umbracoRelations = new HashSet<umbracoRelation>();
            this.umbracoRelations1 = new HashSet<umbracoRelation>();
            this.umbracoUser2NodeNotify = new HashSet<umbracoUser2NodeNotify>();
            this.umbracoUser2NodePermission = new HashSet<umbracoUser2NodePermission>();
            this.umbracoNode11 = new HashSet<umbracoNode>();
            this.umbracoNodes = new HashSet<umbracoNode>();
            this.cmsMembers = new HashSet<cmsMember>();
        }
    
        public int id { get; set; }
        public bool trashed { get; set; }
        public int parentID { get; set; }
        public Nullable<int> nodeUser { get; set; }
        public int level { get; set; }
        public string path { get; set; }
        public int sortOrder { get; set; }
        public Nullable<System.Guid> uniqueID { get; set; }
        public string text { get; set; }
        public Nullable<System.Guid> nodeObjectType { get; set; }
        public System.DateTime createDate { get; set; }
    
        public virtual ICollection<cmsContent> cmsContents { get; set; }
        public virtual ICollection<cmsContentType> cmsContentTypes { get; set; }
        public virtual ICollection<cmsDataType> cmsDataTypes { get; set; }
        public virtual ICollection<cmsDocument> cmsDocuments { get; set; }
        public virtual ICollection<cmsDocumentType> cmsDocumentTypes { get; set; }
        public virtual cmsMember cmsMember { get; set; }
        public virtual ICollection<cmsMemberType> cmsMemberTypes { get; set; }
        public virtual ICollection<cmsPropertyData> cmsPropertyDatas { get; set; }
        public virtual cmsStylesheet cmsStylesheet { get; set; }
        public virtual ICollection<cmsTask> cmsTasks { get; set; }
        public virtual ICollection<cmsTemplate> cmsTemplates { get; set; }
        public virtual ICollection<cmsTemplate> cmsTemplates1 { get; set; }
        public virtual ICollection<umbracoDomain> umbracoDomains { get; set; }
        public virtual ICollection<umbracoNode> umbracoNode1 { get; set; }
        public virtual umbracoNode umbracoNode2 { get; set; }
        public virtual ICollection<umbracoRelation> umbracoRelations { get; set; }
        public virtual ICollection<umbracoRelation> umbracoRelations1 { get; set; }
        public virtual ICollection<umbracoUser2NodeNotify> umbracoUser2NodeNotify { get; set; }
        public virtual ICollection<umbracoUser2NodePermission> umbracoUser2NodePermission { get; set; }
        public virtual ICollection<umbracoNode> umbracoNode11 { get; set; }
        public virtual ICollection<umbracoNode> umbracoNodes { get; set; }
        public virtual ICollection<cmsMember> cmsMembers { get; set; }
    }
}
