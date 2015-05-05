using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using CommonLibrary.Extensions;
using CommonLibrary.Utility;

using InfoTrendsAPI.Map;

using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using InfoTrendsCommon.Exceptions;
using System.Configuration;


namespace InfoTrendsAPI.Methods.Sms
{
    public class Territory
    {
        /// <summary>
        /// Base sql string for polygon
        /// </summary>
        const string SQL_POLYGON = @"
            SELECT 
                id, 
                cbsa, 
                city, 
                state, 
                zipcode, 
                county, 
                markerLat, 
                markerLon, 
                decodedPoints AS points 

            FROM boundaries 
        ";


        public List<InfoTrendsModel.SmsModels.PolygonData> getMultiPolygonData(LatLng latLng, string territoryType)
        {
            MapIdentifier mi = this.GetMapIdentifier(latLng, territoryType);
            if (mi.hasResult == false)
                return null;


            string whereClause = " boundaryType='{0}' "
                .Fmt(territoryType);


            // switch
            switch (territoryType.ToUpper())
            {
                case TerritoryType.CBSA:
                    whereClause += " AND cbsa='{0}' "
                        .Fmt(mi.CBSA);
                    break;

                case TerritoryType.CITY:
                    whereClause += " AND city='{0}' AND state='{1}' "
                        .Fmt(mi.city, mi.state);
                    break;

                case TerritoryType.COUNTY:
                    whereClause += " AND county='{0}' AND state='{1}' "
                        .Fmt(mi.county, mi.state);
                    break;

                case TerritoryType.STATE:
                    whereClause += " AND state='{0}' "
                        .Fmt(mi.state);
                    break;

                case TerritoryType.ZIPCODE:
                    whereClause += " AND zipcode='{0}' "
                        .Fmt(mi.zipcode);
                    break;
            }


            // sql statement
            string sqlStr = "{0} WHERE {1} "
                .Fmt(SQL_POLYGON, whereClause);


            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                    return null;

                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.PolygonData>>(jsonStr);
                return ret;
            }
        }


        /// <summary>
        /// Get polygon data
        /// </summary>
        public InfoTrendsModel.SmsModels.PolygonData getPolygonData(LatLng latLng, string territoryType)
        {
            var ret = getMultiPolygonData(latLng, territoryType);
            if (ret == null)
                return null;
            return ret[0];
        }
        public InfoTrendsModel.SmsModels.PolygonData getPolygonData(InfoTrendsContext context)
        {
            var p = context;
            string territoryType = p.GetParamValue("territory_type");
            string latStr = p.GetParamValue("lat");
            string lngStr = p.GetParamValue("lng");

            double lat;
            double lng;

            // territoryType
            if (territoryType.IsNullOrWhiteSpace())
            {
                throw new MethodException("Invalid TerritoryType");
            }

            // lat
            if (!double.TryParse(latStr, out lat))
            {
                throw new MethodException("Invalid Latitude");
            }

            // lng
            if (!double.TryParse(lngStr, out lng))
            {
                throw new MethodException("Invalid Longitude");
            }


            // return
            LatLng latLng = new LatLng(lat, lng);
            return getPolygonData(latLng, territoryType);
        }


        /// <summary>
        /// Get polygon data by identifier
        /// </summary>
        public List<InfoTrendsModel.SmsModels.PolygonData> getPolygonDataByIdentifier(string territoryType, string cbsa, string city, string county, string state, string zipcode)
        {
            string whereClause = " boundaryType='{0}' "
                .Fmt(territoryType);


            // switch
            switch (territoryType.ToUpper())
            {
                case TerritoryType.CBSA:
                    whereClause += " AND cbsa='{0}' "
                        .Fmt(cbsa);
                    break;

                case TerritoryType.CITY:
                    whereClause += " AND city='{0}' AND state='{1}' "
                        .Fmt(city, state);
                    break;

                case TerritoryType.COUNTY:
                    whereClause += " AND county='{0}' AND state='{1}' "
                        .Fmt(county, state);
                    break;

                case TerritoryType.STATE:
                    whereClause += " AND state='{0}' "
                        .Fmt(state);
                    break;

                case TerritoryType.ZIPCODE:
                    whereClause += " AND zipcode='{0}' "
                        .Fmt(zipcode);
                    break;
            }


            // format
            string sqlStr = "{0} WHERE {1}"
                .Fmt(SQL_POLYGON, whereClause);
            

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0) 
                    return null;

                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.PolygonData>>(jsonStr);
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.PolygonData> getPolygonDataByIdentifier(InfoTrendsContext context)
        {
            var p = context;
            string territoryType = p.GetParamValue("territory_type");
            string cbsa = p.GetParamValue("cbsa");
            string city = p.GetParamValue("city");
            string county = p.GetParamValue("county");
            string state = p.GetParamValue("state");
            string zipcode = p.GetParamValue("zipcode");

            // return
            return getPolygonDataByIdentifier(territoryType, cbsa, city, county, state, zipcode);
        }



        /// <summary>
        /// Region that latLng falls in
        /// </summary>
        public MapIdentifier getIdentifierByLatLng(LatLng latLng, string territoryType)
        {
            MapIdentifier foundIdentifier = GetMapIdentifier(latLng, territoryType);
            return foundIdentifier;
        }
        public MapIdentifier getIdentifierByLatLng(InfoTrendsContext context)
        {
            var p = context;
            string territoryType = p.GetParamValue("territory_type");
            string latStr = p.GetParamValue("lat");
            string lngStr = p.GetParamValue("lng");

            double lat;
            double lng;

            // territoryType
            if (territoryType.IsNullOrWhiteSpace())
            {
                throw new MethodException("Invalid TerritoryType");
            }

            // lat
            if (!double.TryParse(latStr, out lat))
            {
                throw new MethodException("Invalid Latitude");
            }

            // lng
            if (!double.TryParse(lngStr, out lng))
            {
                throw new MethodException("Invalid Longitude");
            }


            // return
            LatLng latLng = new LatLng(lat, lng);
            return getIdentifierByLatLng(latLng, territoryType);
        }



        #region UTILITY

        /// <summary>
        /// Return in what region the latLng falls in
        /// </summary>
        MapIdentifier GetMapIdentifier(LatLng latLng, string territoryType)
        {
            // where clause
            string whereClause = @"
                boundaryType='{0}'
                AND (SWLat <= {1} AND {1} <= NELat)
                AND (SWLon <= {2} AND {2} <= NELon)
            ";
            
            // format where clause
            whereClause = whereClause
                .Fmt(territoryType, latLng.Lat, latLng.Lng);


            // sql string
            string sqlStr = "{0} WHERE {1}"
                .Fmt(SQL_POLYGON, whereClause);

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                // found region
                MapIdentifier foundIdentifier = new MapIdentifier(territoryType);

                //find if the point in inside this region
                foreach (DataRow row in dt.Rows)
                {
                    string pointStr = row["points"].ToString();
                    if (Polygon.isIntersect(pointStr, latLng))
                    {
                        foundIdentifier = MapIdentifier.GetMapIdentifier(row, territoryType);
                        break;
                    }
                }

                //return
                return foundIdentifier;
            }
        }

        #endregion

    }
}
