
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
    
public partial class ENG_WidgetContent_WidgetDataType_Lookup
{

    public int ID { get; set; }

    public Nullable<int> WidgetContentID { get; set; }

    public Nullable<int> WidgetDataTypeID { get; set; }

    public Nullable<System.DateTime> CreateOn { get; set; }

    public Nullable<System.DateTime> ModifyOn { get; set; }



    public virtual ENG_WidgetContent ENG_WidgetContent { get; set; }

    public virtual ENG_WidgetDataType ENG_WidgetDataType { get; set; }

}

}
