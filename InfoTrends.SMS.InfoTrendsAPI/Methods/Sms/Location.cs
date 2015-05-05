using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonLibrary.Extensions;
using CommonLibrary.Utility;
using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using InfoTrendsCommon.Role;
using InfoTrendsAPI.Error;
using InfoTrendsModel.SmsModels;
using System.Configuration;

namespace InfoTrendsAPI.Methods.Sms
{
    public class Location
    {

        /// <summary>
        /// Get location detail
        /// </summary>
        public InfoTrendsModel.SmsModels.LocationDetail get(Guid locationGuid)
        {
            // construct sql string
            string sqlStr = @"
                SELECT *
                FROM {0}
                WHERE locationGuid = '{1}'
            ";


            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.View.SearchCacheDetailViewAll, locationGuid);


            //execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                    return null;

                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.LocationDetail>>(jsonStr);

                // return
                return ret[0];
            }
        }
        //public InfoTrendsModel.SmsModels.LocationDetail get(InfoTrendsContext p)
        //{
        //    // parameters
        //    int epriseId = (int)p.User.epriseId;
        //    Guid locationId = Guid.Parse(p.GetParamValue("location_guid"));
        //    return get(p.RoleList, locationId);
        //}



        /// <summary>
        /// Get location public note
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<InfoTrendsModel.SmsModels.LocationNote> getPublicNote(Guid locationGuid)
        {
            string sqlStr = @"
                SELECT * 
                FROM {0} 
                WHERE locationGUID = '{1}'
            ";

            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.View.NotesView, locationGuid);

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                    return null;

                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.LocationNote>>(jsonStr);
                
                // return
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.LocationNote> getPublicNote(InfoTrendsContext p)
        {
            // parameters
            int epriseId = (int)p.User.epriseId;
            Guid locationId = Guid.Parse(p.GetParamValue("location_guid"));

            //return
            return getPublicNote(locationId);
        }



        /// <summary>
        /// Search for locations
        /// </summary>
        public List<InfoTrendsModel.SmsModels.Location> search(Guid? contactId, List<RoleItem> roleList, string filter)
        {
            // extra filter from sales order
            //string extraFilter = GetRestrictedSqlCondition(roleList, contactId);
            //extraFilter = extraFilter.IsNullOrWhiteSpace() ? "" : " AND " + extraFilter;
            string extraFilter = "";

            // construct sql string
            string sqlStr = @"
                SELECT 
                    locationID, 
                    locationGUID, 
                    parentID, 
                    latitude, 
                    longitude, 
                    locationName, 
                    locationType, 
                    locationURL, 
                    facebook, 
                    twitter, 
                    linkedin, 
                    urlIframe

                FROM 
                    {0}
                WHERE 
                    {1} 
                    {2}
            ";


            // format
            sqlStr = sqlStr.Fmt(
                DbConstant.SmsDb.View.SearchCacheDetailViewAll,
                filter,
                extraFilter
                );


            //execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.Location>>(jsonStr);
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.Location> search(InfoTrendsContext p)
        {
            // parameters
            string filter = p.GetParamValue("filter");
            return search(p.User.crmContactGUID, p.RoleList, filter);
        }



        #region PUBLIC STATIC


        #endregion



        #region UTILITY


        /// <summary>
        /// Whether to apply SmsCommercialChannel data only
        /// which restricted data from having '_Retail_'
        /// </summary>
        /// <param name="sesion"></param>
        /// <returns></returns>
        static string GetRetailRestrictedCondition(bool isSmsCommercialChannel)
        {
            string ret = "";

            if (isSmsCommercialChannel == true)
            {
                ret = " NOT (dbo.regExMatch(quickSearch,'_Retail_') = 1) ";
            }

            // return
            return ret;
        }


        #endregion




    }
}
