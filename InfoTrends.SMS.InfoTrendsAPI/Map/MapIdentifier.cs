using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;


namespace InfoTrendsAPI.Map
{
    public class MapIdentifier
    {

        public bool hasResult { get; set;  }

        public string CBSA { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string zipcode { get; set; }
        public string type { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public MapIdentifier()
        {
            CBSA = "";
            city = "";
            state = "";
            county = "";
            zipcode = "";
            type = "";
            hasResult = false;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public MapIdentifier(string type)
            : this()
        {
            type = type.ToUpper();
        }




        #region " STATIC "


        public static MapIdentifier GetMapIdentifier(DataRow data, string territoryType)
        {
            MapIdentifier mi = new MapIdentifier(territoryType);
            mi.CBSA = data["CBSA"].ToString();
            mi.city = data["city"].ToString();
            mi.county = data["county"].ToString();
            mi.zipcode = data["zipcode"].ToString();
            mi.state = data["state"].ToString();
            mi.type = territoryType.ToString();
            mi.hasResult = true;
            return mi;
        }


        #endregion


    }
}
