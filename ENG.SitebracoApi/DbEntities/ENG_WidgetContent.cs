
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
    
public partial class ENG_WidgetContent
{

    public int ID { get; set; }

    public Nullable<int> WidgetId { get; set; }

    public Nullable<int> ContentId { get; set; }

    public string Name { get; set; }

    public string Color { get; set; }

    public Nullable<int> WidgetDataId { get; set; }



    public virtual ENG_Widget ENG_Widget { get; set; }

    public virtual ENG_WidgetData ENG_WidgetData { get; set; }

}

}
