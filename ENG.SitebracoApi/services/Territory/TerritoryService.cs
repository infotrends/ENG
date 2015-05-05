using SitebracoApi.Models.Territory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InfoTrendsAPI.Map;
using InfoTrendsCommon.Exceptions;
using InfoTrendsCommon.OAuth;
using SitebracoApi.Common;

namespace SitebracoApi.Services.Territory
{
    public class TerritoryService
    {
        /// <summary>
        /// Base sql string for polygon
        /// </summary>
        private const string SQL_POLYGON = @"
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

        public List<PolygonData> getMultiPolygonData(LatLng latLng, string territoryType)
        {
            MapIdentifier mi = this.GetMapIdentifier(latLng, territoryType);
            if (mi.hasResult == false)
                return null;

            string whereClause = string.Format("boundaryType='{0}' ", territoryType);

            // switch
            switch (territoryType.ToUpper())
            {
                case TerritoryType.CBSA:
                    whereClause += string.Format(" AND cbsa='{0}' ", mi.CBSA);
                    break;

                case TerritoryType.CITY:
                    whereClause += string.Format(" AND city='{0}' AND state='{1}' ", mi.city, mi.state);
                    break;

                case TerritoryType.COUNTY:
                    whereClause += string.Format(" AND county='{0}' AND state='{1}' ", mi.county, mi.state);
                    break;

                case TerritoryType.STATE:
                    whereClause += string.Format(" AND state='{0}' ", mi.state);
                    break;

                case TerritoryType.ZIPCODE:
                    whereClause += string.Format(" AND zipcode='{0}' ", mi.zipcode);
                    break;
            }


            // sql statement
            string sqlStr = string.Format("{0} WHERE {1} ", SQL_POLYGON, whereClause);


            var connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            // execute

            try
            {
                SqlCommand cmd = new SqlCommand(sqlStr, connection);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return null;

                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<List<PolygonData>>(jsonStr);
                return ret;
            }
            catch (Exception ex)
            {
                //logger.Error(typeof(TerritoryService).Name + ": Method:(" + System.Reflection.MethodBase.GetCurrentMethod().Name + "): " + ex.Message);

                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Get polygon data
        /// 
        /// </summary>
        public PolygonData getPolygonData(LatLng latLng, string territoryType)
        {
            var ret = getMultiPolygonData(latLng, territoryType);
            return ret == null ? null : ret[0];
        }

        public PolygonData getPolygonData(InfoTrendsContext context)
        {
            var p = context;
            string territoryType = p.GetParamValue("territory_type");
            string latStr = p.GetParamValue("lat");
            string lngStr = p.GetParamValue("lng");

            double lat;
            double lng;

            // territoryType
            if (string.IsNullOrWhiteSpace(territoryType))
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
        public List<PolygonData> getPolygonDataByIdentifier(string territoryType, string cbsa,
            string city, string county, string state, string zipcode)
        {
            var whereClause = string.Format(" boundaryType='{0}' ", territoryType);


            // switch
            switch (territoryType.ToUpper())
            {
                case TerritoryType.CBSA:
                    whereClause += string.Format(" AND cbsa='{0}' ", cbsa);
                    break;

                case TerritoryType.CITY:
                    whereClause += string.Format(" AND city='{0}' AND state='{1}' ", city, state);
                    break;

                case TerritoryType.COUNTY:
                    whereClause += string.Format(" AND county='{0}' AND state='{1}' ", county, state);
                    break;

                case TerritoryType.STATE:
                    whereClause += string.Format(" AND state='{0}' ", state);
                    break;

                case TerritoryType.ZIPCODE:
                    whereClause += string.Format(" AND zipcode='{0}' ", zipcode);
                    break;
            }


            // format
            string sqlStr = string.Format("{0} WHERE {1}", SQL_POLYGON, whereClause);
            var connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlStr, connection);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return null;
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<List<PolygonData>>(jsonStr);
                return ret;
            }
            catch (Exception ex)
            {
                //logger.Error(typeof(TerritoryService).Name + ": Method:(" + System.Reflection.MethodBase.GetCurrentMethod().Name + "): " + ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<PolygonData> getPolygonDataByIdentifier(InfoTrendsContext context)
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
            if (string.IsNullOrWhiteSpace(territoryType))
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
        private MapIdentifier GetMapIdentifier(LatLng latLng, string territoryType)
        {
            // where clause
            string whereClause = @"
                boundaryType='{0}'
                AND (SWLat <= {1} AND {1} <= NELat)
                AND (SWLon <= {2} AND {2} <= NELon)
            ";

            // format where clause
            whereClause = string.Format(whereClause, territoryType, latLng.Lat, latLng.Lng);
            var connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            var connection = new SqlConnection(connectionString);

            // sql string
            string sqlStr = string.Format("{0} WHERE {1}", SQL_POLYGON, whereClause);

            try
            {
                SqlCommand cmd = new SqlCommand(sqlStr, connection);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
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
            catch (Exception ex)
            {
                //logger.Error(typeof(TerritoryService).Name + ": Method:(" + System.Reflection.MethodBase.GetCurrentMethod().Name + "): " + ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion
    }
}
