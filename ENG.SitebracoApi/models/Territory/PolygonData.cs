using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Territory
{
    public class PolygonData
    {
        public string cbsa { get; set; }

        public string city { get; set; }

        public string county { get; set; }

        public int? id { get; set; }

        public double? markerLat { get; set; }

        public double? markerLon { get; set; }

        public string points { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public string nutID { get; set; }

        public string canID { get; set; }
        
        public string name { get; set; }

        public string color { get; set; }
    }
}
