using InfoTrendsAPI.Map;
using SitebracoApi.Models.Territory;
using SitebracoApi.Services.Territory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.DbEntities;
using System.Web.Http;

namespace SitebracoApi.Controllers.Territory
{
    public class TerritoryController : BaseController
    {
        #region Properties

        public TerritoryService TerritoryService = new TerritoryService();
        #endregion


        #region Action

        /// <summary>
        /// Get Multi Polygon Data
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="territoryType"></param>
        /// <returns></returns>
        public IEnumerable<PolygonData> getMultiPolygonData(double lat, double lng, string territoryType)
        {

            var latlgn = new LatLng(lat, lng);
            return TerritoryService.getMultiPolygonData(latlgn, territoryType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="territoryType"></param>
        /// <returns></returns>
        public IEnumerable<PolygonData> getMultiPolygonData(string data, string territoryType)
        {
            if (territoryType != null && territoryType.IndexOf("CAN_", StringComparison.Ordinal) != -1)
            {
                return multiPolygonDataCan(data, territoryType);
            }
            using (var database = new SitebracoEntities())
            {
                var latLgn = Newtonsoft.Json.JsonConvert.DeserializeObject<LatLng>(data);
                var lat = latLgn.Lat;
                var lng = latLgn.Lng;

                var polygons = database.NUTSBoundaries.Where(c => c.boundaryType == territoryType && c.NELat >= lat && c.SWLat <= lat && c.NELon >= lng && c.SWLon <= lng);

                if (polygons == null || polygons.Count() == 0)
                    return null;
                var polygonsReturn = new List<PolygonData>();
                foreach (var p in polygons)
                {
                    if (Polygon.isIntersect(p.Points, latLgn))
                    {
                        return database.NUTSBoundaries.Where(c => c.NUTS_ID == p.NUTS_ID).Select(c => new PolygonData { id = c.Id, points = c.Points, nutID = p.NUTS_ID });
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get polygon data by identifier
        /// </summary>
        /// <param name="territoryType"></param>
        /// <param name="cbsa"></param>
        /// <param name="city"></param>
        /// <param name="county"></param>
        /// <param name="state"></param>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public List<PolygonData> getPolygonDataByIdentifier(string territoryType, string cbsa, string city, string county, string state, string zipcode)
        {
            return TerritoryService.getPolygonDataByIdentifier(territoryType, cbsa, city, county, state, zipcode);
        }

        /// <summary>
        /// Get cbsa detail
        /// </summary>
        /// <param name="territoryID"></param>
        /// <param name="territoryType"></param>
        /// <returns></returns>
        public List<TerritoryDetail> getPolygonsByTerritory(int territoryID, string territoryType)
        {
            using (var database = new SitebracoEntities())
            {
                string sqlStr = @"SELECT t.territoryID, t.name, t.boundaryType, t.color, t.mapInfo, b.id ,b.state, b.cbsa, 
                        b.cbsacode, b.SWLat, b.SWLon, b.NELat, b.NELon, b.points, b.levels, b.decodedPoints, s.BPI, 
                        s.CBSARank10, s.CBSAPopulation10 
                        from [dbo].[TerritoryO] as t
	                    INNER JOIN [dbo].[territoryGrid] as tg
	                    on t.territoryID = tg.territoryID
	                    INNER JOIN [dbo].[boundaries] as b
	                    on b.id = tg.boundaryID
	                    INNER JOIN [dbo].[CBSASummary2] as s
	                    on b.cbsacode = s.CBSACode + ''
                        WHERE 
                        t.territoryID = @territoryID and b.cbsacode is not null and t.boundaryType = @territoryType";

                return database.Database.SqlQuery<TerritoryDetail>(sqlStr, new SqlParameter("@territoryID", territoryID),
                        new SqlParameter("@territoryType", territoryType)).ToList();
            }

        }

        // <summary>
        /// Get boundary and nut boundary by Id
        /// </summary>
        /// <param name="boundary id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IEnumerable<PolygonData> postBoundaryById(BoundaryParams param)
        {
            using (var database = new SitebracoEntities())
            {
                if (param.layerType == null)
                {
                    return null;
                }

                List<PolygonData> listData = new List<PolygonData>();
                if (param.layerType == "nuts")
                {
                    listData = database.NUTSBoundaries
                        .Where(i => param.nutsId.Contains(i.NUTS_ID))
                        .Select(i => new PolygonData { id = i.Id, nutID = i.NUTS_ID, points = i.Points }).ToList();
                }
                else if (param.layerType.Contains("CAN_"))
                {
                    dynamic data = null;
                    if (param.layerType.Equals("CAN_PR"))
                    {
                        data = database.mt_ca_province_2014a_boundaries
                        .Where(i => param.canId.Contains(i.Id))
                        .Select(i => new { id = i.Id, canID = i.capabbr, name = i.prename, markerLat = i.labellat, markerLon = i.labellon, points = i.the_geom, color = i.color }).ToList();
                    }
                    else if (param.layerType.Equals("CAN_FSA"))
                    {
                        data = database.mt_ca_fsa_2014a_boundaries
                        .Where(i => param.canId.Contains(i.Id))
                        .Select(i => new { id = i.Id, canID = i.cfsauid, name = i.prename, markerLat = i.labellat, markerLon = i.labellon, points = i.the_geom, color = i.color }).ToList();
                    }
                    else
                    {
                        return null;
                    }
                    foreach (dynamic t in data)
                    {
                        string border;
                        if (t.points.SpatialTypeName == "Polygon")
                        {
                            border = t.points.ToString();
                            border = border.Substring(border.LastIndexOf("(") + 1);
                            border = border.Substring(0, border.IndexOf(")"));
                            border = border.Replace(", ", ";").Replace(" ", ",");
                            PolygonData item = new PolygonData();
                            item.id = t.id;
                            item.canID = t.canID;
                            item.name = t.name;
                            item.markerLat = t.markerLat;
                            item.markerLon = t.markerLon;
                            item.points = border;
                            listData.Add(item);
                        }
                        else if (t.points.SpatialTypeName == "MultiPolygon")
                        {
                            for (var i = 1; i <= t.points.ElementCount; i++)
                            {

                                var innerT = t.points.ElementAt(i);
                                border = innerT.ToString();
                                border = border.Substring(border.LastIndexOf("(") + 1);
                                border = border.Substring(0, border.IndexOf(")"));
                                border = border.Replace(", ", ";").Replace(" ", ",");

                                PolygonData item = new PolygonData();
                                item.id = t.id;
                                item.canID = t.canID;
                                item.name = t.name;
                                item.markerLat = t.markerLat;
                                item.markerLon = t.markerLon;
                                item.points = border;
                                listData.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    listData = database.boundaries
                        .Where(i => param.layersId.Contains(i.id))
                        .Select(i => new PolygonData { id = i.id, points = i.decodedPoints, cbsa = i.cbsa, city = i.city, county = i.county, markerLon = i.markerLon, markerLat = i.markerLat, state = i.state, zipcode = i.zipcode }).ToList();
                }
                return listData;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="territoryType"></param>
        /// <returns></returns>
        public IEnumerable<PolygonData> multiPolygonDataCan(string data, string territoryType)
        {
            using (var database = new SitebracoEntities())
            {
                var latLgn = Newtonsoft.Json.JsonConvert.DeserializeObject<LatLng>(data);
                var lat = latLgn.Lat;
                var lng = latLgn.Lng;
                dynamic territories = null;
                if (territoryType != null)
                {
                    if (territoryType == "CAN_FSA")
                    {
                        territories = database.mt_ca_fsa_2014a_boundaries.Where(c => c.intptlatn >= lat && c.intptlats <= lat && c.intptlone >= lng && c.intptlonw <= lng)
                            .Select(c => new { id = c.Id, canID = c.cfsauid, name = c.prename, markerLat = c.labellat, markerLon = c.labellon, points = c.the_geom, color = c.color }).ToList();
                    }
                    else if (territoryType == "CAN_PR")
                    {
                        territories = database.mt_ca_province_2014a_boundaries.Where(c => c.intptlatn >= lat && c.intptlats <= lat && c.intptlone >= lng && c.intptlonw <= lng)
                        .Select(c => new { id = c.Id, canID = c.capabbr, name = c.prename, markerLat = c.labellat, markerLon = c.labellon, points = c.the_geom, color = c.color }).ToList();
                    }
                }
                else
                {
                    return null;
                }
                List<PolygonData> listPolygonsData = new List<PolygonData>();
                if (territories != null)
                {
                    var check = false;
                    foreach (dynamic t in territories)
                    {
                        string border;
                        if (t.points.SpatialTypeName == "Polygon")
                        {
                            border = t.points.ToString();
                            border = border.Substring(border.LastIndexOf("(") + 1);
                            border = border.Substring(0, border.IndexOf(")"));
                            border = border.Replace(", ", ";").Replace(" ", ",");
                            if (Polygon.isIntersectGeometry(border, latLgn))
                            {
                                check = true;
                            }
                            if (check == true)
                            {
                                PolygonData item = new PolygonData();
                                item.id = t.id;
                                item.canID = t.canID;
                                item.name = t.name;
                                item.markerLat = t.markerLat;
                                item.markerLon = t.markerLon;
                                item.points = border;
                                listPolygonsData.Add(item);
                            }
                        }
                        else if (t.points.SpatialTypeName == "MultiPolygon")
                        {
                            for (var i = 1; i <= t.points.ElementCount; i++)
                            {
                                var innerT = t.points.ElementAt(i);
                                border = innerT.ToString();
                                border = border.Substring(border.LastIndexOf("(") + 1);
                                border = border.Substring(0, border.IndexOf(")"));
                                border = border.Replace(", ", ";").Replace(" ", ",");
                                if (Polygon.isIntersectGeometry(border, latLgn))
                                {
                                    check = true;
                                    break;
                                }
                            }
                            if (check == true)
                            {
                                for (var i = 1; i <= t.points.ElementCount; i++)
                                {

                                    var innerT = t.points.ElementAt(i);
                                    border = innerT.ToString();
                                    border = border.Substring(border.LastIndexOf("(") + 1);
                                    border = border.Substring(0, border.IndexOf(")"));
                                    border = border.Replace(", ", ";").Replace(" ", ",");

                                    PolygonData item = new PolygonData();
                                    item.id = t.id;
                                    item.canID = t.canID;
                                    item.name = t.name;
                                    item.markerLat = t.markerLat;
                                    item.markerLon = t.markerLon;
                                    item.points = border;
                                    listPolygonsData.Add(item);
                                }
                            }
                        }
                        if (check)
                        {
                            break;
                        }
                    }
                }
                return listPolygonsData;
            }

        }

        #endregion
    }
}
