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
    public class SearchHistory
    {

        /// <summary>
        /// Create save search from specific user
        /// </summary>
        public Int64 add(int userId, string name, string filter, string mapInfo, string color, string note)
        {
            //check to see whether history limit is reached
            //if reached, delete the oldest entry
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1} ORDER BY dateCreated DESC "
                .Fmt(DbConstant.SmsDb.Table.SearchHistory, userId);
            using (DataTable histEntries = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                int histCount = histEntries.Rows.Count;
                if (histCount == 20)
                {
                    DataRow dr = histEntries.Rows[histCount - 1];
                    Int64 recId = (Int64)dr[0];
                    delete(userId, recId);
                }
            }


            //add new history entry
            sqlStr = "  INSERT INTO {0} (epriseID,  name,  filter,  mapInfo,  color,  note) ";
            sqlStr += "          VALUES ({1},      @name, @filter, @mapInfo, @color, @note) ";
            sqlStr += SqlStatement.SQL_GET_SCOPE_IDENTITY;


            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.SearchHistory, userId);


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@filter", filter));
                sqlCmd.Parameters.Add(new SqlParameter("@mapInfo", mapInfo));
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                        throw new MethodException(SmsError.HISTORY_COULD_NOT_BE_CREATED);

                    Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());

                    return newID;
                }
            }
        }
        public Int64 add(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string name = p.GetParamValue("name");
            string filter = p.GetParamValue("filter");
            string mapInfo = p.GetParamValue("map_info");
            string color = p.GetParamValue("color");
            string note = p.GetParamValue("note");

            return add(epriseId, name, filter, mapInfo, color, note);
        }


        /// <summary>
        /// Delete specific saved search of specfic user
        /// </summary>
        public bool delete(int userId, Int64 recId)
        {
            string sqlStr = "DELETE FROM {0} WHERE epriseID={1} AND ID={2} "
                .Fmt(DbConstant.SmsDb.Table.SearchHistory, userId, recId);

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
        /// Return all saved searches of the specific user
        /// </summary>
        public List<InfoTrendsModel.SmsModels.SearchHistory> list(int userId)
        {
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1} ORDER BY dateCreated DESC"
                .Fmt(DbConstant.SmsDb.Table.SearchHistory, userId);

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SearchHistory>>(jsonStr);
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.SearchHistory> list(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;

            return list(epriseId);
        }


        /// <summary>
        /// Update history
        /// </summary>
        public bool update(int userId, Int64 recId, string color, bool saved)
        {
            color = SqlStatement.Normalize(color);

            string sqlStr = "";
            sqlStr += " UPDATE {0} SET color=@color, saved=@saved WHERE epriseID={1} AND ID={2}";
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.SearchHistory, userId, recId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@saved", saved));
                SqlServerUtil.ExecCommand(sqlCmd);
                return true;
            }
        }
        public bool update(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string color = p.GetParamValue("color");
            string note = p.GetParamValue("note");
            string recIdStr = p.GetParamValue("rec_id");

            Int64 recId;
            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            return update(epriseId, recId, color, true);
        }

    }
}
