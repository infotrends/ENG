using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Map
{
    public class LatLng
    {
        /// <summary>
        /// properties
        /// </summary>
        double _lat;
        double _lng;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public LatLng(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public LatLng() { }

        /// <summary>
        /// getter & setter
        /// </summary>
        public double Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                if (value > -90.0 && value < 90.00)
                {
                    _lat = value;
                }
            }
        }


        /// <summary>
        /// getter & setter
        /// </summary>
        public double Lng
        {
            get
            {
                return _lng;
            }
            set
            {
                if (value >= -180.0 && value <= 180.0)
                {
                    this._lng = value;
                }
            }
        }
    }
}
