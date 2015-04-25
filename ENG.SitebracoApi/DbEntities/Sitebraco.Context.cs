﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;


public partial class SitebracoEntities : DbContext
{
    public SitebracoEntities()
        : base("name=SitebracoEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<Action> Actions { get; set; }

    public DbSet<cmsContent> cmsContents { get; set; }

    public DbSet<cmsContentType> cmsContentTypes { get; set; }

    public DbSet<cmsContentTypeAllowedContentType> cmsContentTypeAllowedContentTypes { get; set; }

    public DbSet<cmsContentVersion> cmsContentVersions { get; set; }

    public DbSet<cmsContentXml> cmsContentXmls { get; set; }

    public DbSet<cmsDataType> cmsDataTypes { get; set; }

    public DbSet<cmsDataTypePreValue> cmsDataTypePreValues { get; set; }

    public DbSet<cmsDictionary> cmsDictionaries { get; set; }

    public DbSet<cmsDocument> cmsDocuments { get; set; }

    public DbSet<cmsDocumentType> cmsDocumentTypes { get; set; }

    public DbSet<CMSImportCustomRelation> CMSImportCustomRelations { get; set; }

    public DbSet<CMSImportMediaRelation> CMSImportMediaRelations { get; set; }

    public DbSet<CMSImportRelation> CMSImportRelations { get; set; }

    public DbSet<CMSImportScheduledItem> CMSImportScheduledItems { get; set; }

    public DbSet<CMSImportScheduledTask> CMSImportScheduledTasks { get; set; }

    public DbSet<CMSImportState> CMSImportStates { get; set; }

    public DbSet<cmsLanguageText> cmsLanguageTexts { get; set; }

    public DbSet<cmsMacro> cmsMacroes { get; set; }

    public DbSet<cmsMacroProperty> cmsMacroProperties { get; set; }

    public DbSet<cmsMember> cmsMembers { get; set; }

    public DbSet<cmsMemberType> cmsMemberTypes { get; set; }

    public DbSet<cmsPreviewXml> cmsPreviewXmls { get; set; }

    public DbSet<cmsPropertyData> cmsPropertyDatas { get; set; }

    public DbSet<cmsPropertyType> cmsPropertyTypes { get; set; }

    public DbSet<cmsPropertyTypeGroup> cmsPropertyTypeGroups { get; set; }

    public DbSet<cmsStylesheet> cmsStylesheets { get; set; }

    public DbSet<cmsStylesheetProperty> cmsStylesheetProperties { get; set; }

    public DbSet<cmsTagRelationship> cmsTagRelationships { get; set; }

    public DbSet<cmsTag> cmsTags { get; set; }

    public DbSet<cmsTask> cmsTasks { get; set; }

    public DbSet<cmsTaskType> cmsTaskTypes { get; set; }

    public DbSet<cmsTemplate> cmsTemplates { get; set; }

    public DbSet<Component> Components { get; set; }

    public DbSet<ContentSelector> ContentSelectors { get; set; }

    public DbSet<DataSnapshot> DataSnapshots { get; set; }

    public DbSet<Domain> Domains { get; set; }

    public DbSet<new_MetaClassPrimaryLookup> new_MetaClassPrimaryLookup { get; set; }

    public DbSet<new_OfficeTemplate> new_OfficeTemplate { get; set; }

    public DbSet<new_OfficeTemplateVersion> new_OfficeTemplateVersion { get; set; }

    public DbSet<ProfilingAttribute> ProfilingAttributes { get; set; }

    public DbSet<Report> Reports { get; set; }

    public DbSet<Segment> Segments { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<sysdiagram> sysdiagrams { get; set; }

    public DbSet<TriggerAction> TriggerActions { get; set; }

    public DbSet<TriggerRule> TriggerRules { get; set; }

    public DbSet<TriggerRuleType> TriggerRuleTypes { get; set; }

    public DbSet<Trigger> Triggers { get; set; }

    public DbSet<umbracoDomain> umbracoDomains { get; set; }

    public DbSet<umbracoLanguage> umbracoLanguages { get; set; }

    public DbSet<umbracoLog> umbracoLogs { get; set; }

    public DbSet<umbracoNode> umbracoNodes { get; set; }

    public DbSet<umbracoRelation> umbracoRelations { get; set; }

    public DbSet<umbracoRelationType> umbracoRelationTypes { get; set; }

    public DbSet<umbracoServer> umbracoServers { get; set; }

    public DbSet<umbracoUser> umbracoUsers { get; set; }

    public DbSet<umbracoUser2app> umbracoUser2app { get; set; }

    public DbSet<umbracoUser2NodeNotify> umbracoUser2NodeNotify { get; set; }

    public DbSet<umbracoUser2NodePermission> umbracoUser2NodePermission { get; set; }

    public DbSet<umbracoUserLogin> umbracoUserLogins { get; set; }

    public DbSet<umbracoUserType> umbracoUserTypes { get; set; }

    public DbSet<VisitorComponent> VisitorComponents { get; set; }

    public DbSet<Visitor> Visitors { get; set; }

    public DbSet<VisitorSegment> VisitorSegments { get; set; }

    public DbSet<VisitorTriggerRule> VisitorTriggerRules { get; set; }

    public DbSet<new_MetaClassPrimaryView> new_MetaClassPrimaryView { get; set; }

    public DbSet<ENG_OS> ENG_OS { get; set; }

    public DbSet<ENG_Session> ENG_Session { get; set; }

    public DbSet<ENG_TrackingList> ENG_TrackingList { get; set; }

    public DbSet<ENG_UserSetting> ENG_UserSetting { get; set; }

    public DbSet<ENG_WidgetContent> ENG_WidgetContent { get; set; }

    public DbSet<ENG_WidgetData> ENG_WidgetData { get; set; }

    public DbSet<ENG_WidgetSetting> ENG_WidgetSetting { get; set; }

    public DbSet<ENG_Widget> ENG_Widget { get; set; }

    public DbSet<ENG_WidgetDataType> ENG_WidgetDataType { get; set; }

    public DbSet<ENG_WidgetSubscribe> ENG_WidgetSubscribe { get; set; }

    public DbSet<ENG_DashboardSettingColumn> ENG_DashboardSettingColumn { get; set; }

    public DbSet<ENG_DashboardSettingReport> ENG_DashboardSettingReport { get; set; }

    public DbSet<ENG_WidgetContent_WidgetDataType_Lookup> ENG_WidgetContent_WidgetDataType_Lookup { get; set; }

    public DbSet<ENG_DashboardSettingView> ENG_DashboardSettingView { get; set; }


    public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var versionParameter = version.HasValue ?
            new ObjectParameter("version", version) :
            new ObjectParameter("version", typeof(int));


        var definitionParameter = definition != null ?
            new ObjectParameter("definition", definition) :
            new ObjectParameter("definition", typeof(byte[]));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
    }


    public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var versionParameter = version.HasValue ?
            new ObjectParameter("version", version) :
            new ObjectParameter("version", typeof(int));


        var definitionParameter = definition != null ?
            new ObjectParameter("definition", definition) :
            new ObjectParameter("definition", typeof(byte[]));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
    }


    public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
    }


    public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
    }


    public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
    }


    public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var new_diagramnameParameter = new_diagramname != null ?
            new ObjectParameter("new_diagramname", new_diagramname) :
            new ObjectParameter("new_diagramname", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
    }


    public virtual int sp_upgraddiagrams()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
    }

}

}

