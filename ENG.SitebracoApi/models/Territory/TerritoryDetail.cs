using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models.Territory
{
    public class TerritoryDetail
    {
        public int territoryID { get; set; }

        public string name { get; set; }

        public string boundaryType { get; set; }

        public string color { get; set; }

        public string mapInfo { get; set; }

        public string state { get; set; }

        public string cbsa { get; set; }

        public string cbsacode { get; set; }

        public double? sWLat { get; set; }

        public double? sWLon { get; set; }

        public double? nELat { get; set; }

        public double? nELon { get; set; }

        public int? id { get; set; }

        public string points { get; set; }

        public string levels { get; set; }

        public string decodedPoints { get; set; }

        public double? bPI { get; set; }

        public int? cBSARank10 { get; set; }

        public long? cBSAPopulation10 { get; set; }
    }
}
