
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
    
public partial class NUTSPolygonData
{

    public int Id { get; set; }

    public Nullable<int> Level { get; set; }

    public string NUTS_ID { get; set; }

    public Nullable<int> PointID { get; set; }

    public Nullable<int> PolygonID { get; set; }

    public Nullable<int> SubPolygonID { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public Nullable<int> Number_of_Records { get; set; }

    public Nullable<double> SHAPE_AREA { get; set; }

    public Nullable<double> SHAPE_LEN { get; set; }

}

}
