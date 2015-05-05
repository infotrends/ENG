using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Map
{
    public class LatLngBounds
    {
        public LatLng SouthWest
        { get; set; }


        public LatLng NorthEast
        { get; set; }


        public LatLngBounds(LatLng sw, LatLng ne)
        {
            this.SouthWest = sw;
            this.NorthEast = ne;
        }
        

    }
}
