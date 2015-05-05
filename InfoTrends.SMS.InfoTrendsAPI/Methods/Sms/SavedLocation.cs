using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CommonLibrary.Extensions;
using CommonLibrary.DataContext;
using CommonLibrary.Utility;
using InfoTrendsModel.SmsModels;
using InfoTrendsCommon.Exceptions;
using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using InfoTrendsAPI.Error;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;


namespace InfoTrendsAPI.Methods.Sms
{
    public class SavedLocation
    {

        /// <summary>
        /// Create saved location from specific user
        /// </summary>
        public Int64 add(int userId, Int32 locationId, string note)
        {
            string sqlStr = "INSERT INTO {0} (epriseID, locationID, note) VALUES ({1}, {2}, @note)";
            sqlStr += SqlStatement.SQL_GET_SCOPE_IDENTITY;

            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.SavedLocations, userId, locationId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));
                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                        throw new MethodException("Location could not be created");

                    Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());

                    return newID;
                }
            }
        }
        public Int64 add(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string locationIdStr = p.GetParamValue("location_id");
            string note = p.GetParamValue("note");

            int locationId;

            if (!int.TryParse(locationIdStr, out locationId))
            {
                throw new MethodException("Invalid Location Id");
            }

            return add(epriseId, locationId, note);
        }


        /// <summary>
        /// Delete specific saved locations from specific user
        /// </summary>
        public bool delete(int userId, Int64 recId)
        {
            string sqlStr = "DELETE FROM {0} WHERE epriseID={1} AND ID={2}"
                                .Fmt(DbConstant.SmsDb.Table.SavedLocations, userId, recId);

            SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);

            return true;
        }
        public bool delete(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string recIdStr = p.GetParamValue("rec_id");

            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            return delete(epriseId, recId);
        }


        /// <summary>
        /// Return all save locations of specific user
        /// </summary>
        public List<InfoTrendsModel.SmsModels.SavedLocation> list(int userId)
        {
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1}"
                .Fmt(DbConstant.SmsDb.View.SavedLocationsView, userId);

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedLocation>>(jsonStr);

                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.SavedLocation> list(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            return list(epriseId);
        }


        /// <summary>
        /// Update location of specific user
        /// </summary>
        public bool update(int userId, Int64 recId, string note)
        {
            string sqlStr = "UPDATE {0} SET note=@note WHERE epriseID={1} AND ID={2}"
                .Fmt(DbConstant.SmsDb.Table.SavedLocations, userId, recId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));
                SqlServerUtil.ExecCommand(sqlCmd);
                return true;
            }
        }
        public bool update(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string recIdStr = p.GetParamValue("rec_id");
            string note = p.GetParamValue("note");

            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            return update(epriseId, recId, note);
        }

    }
}
