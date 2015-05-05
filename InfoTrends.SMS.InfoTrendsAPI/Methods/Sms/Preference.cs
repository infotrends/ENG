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
    public class Preference
    {
        /// <summary>
        /// Create new settings of specific user
        /// </summary>
        public Int64 add(int userId, string name, string data)
        {
            string sqlStr = "";
            sqlStr += " INSERT INTO {0} (epriseID,  name,  data) ";
            sqlStr += "          VALUES ({1},      @name, @data)";
            sqlStr += SqlStatement.SQL_GET_SCOPE_IDENTITY;

            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserSettings, userId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@data", data));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                        throw new MethodException(SmsError.SETTINGS_COULD_NOT_BE_CREATED);

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
            string data = p.GetParamValue("data");

            return add(epriseId, name, data);
        }


        /// <summary>
        /// Return setting of specific user
        /// </summary>
        public InfoTrendsModel.SmsModels.Preference get(int userId, string name)
        {
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1} AND name=@name"
                .Fmt(DbConstant.SmsDb.Table.UserSettings, userId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt.Rows.Count == 0)
                        return null;

                    string jsonStr = JsonConvert.SerializeObject(dt);
                    var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.Preference>>(jsonStr);
                    return ret[0];
                }
            }

        }
        public InfoTrendsModel.SmsModels.Preference get(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string name = p.GetParamValue("name");

            return get(epriseId, name);
        }


        /// <summary>
        /// Return all settings of specific user
        /// </summary>
        public List<InfoTrendsModel.SmsModels.Preference> list(int userId)
        {
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1}"
                .Fmt(DbConstant.SmsDb.Table.UserSettings, userId);

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.Preference>>(jsonStr);
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.Preference> list(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            return list(epriseId);
        }


        /// <summary>
        /// Update settings of specific user
        /// </summary>
        public bool update(int userId, string name, string data)
        {
            string sqlStr = "UPDATE {0} SET data=@data WHERE epriseID={1} AND name=@name"
                .Fmt(DbConstant.SmsDb.Table.UserSettings, userId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@data", data));
                SqlServerUtil.ExecCommand(sqlCmd);
                return true;
            }
        }
        public bool update(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string name = p.GetParamValue("name");
            string data = p.GetParamValue("data");

            return update(epriseId, name, data);
        }



        /// <summary>
        /// Add preference => if not exist else update
        public bool save(int userId, string name, string data)
        {
            string sqlStr = "SELECT COUNT(ID) FROM {0} WHERE epriseID={1} AND name=@name"
                .Fmt(DbConstant.SmsDb.Table.UserSettings, userId);

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                int count = (int)SqlServerUtil.ExecScalar(sqlCmd);
                bool existPreference = count > 0;

                if (existPreference == true)
                {
                    update(userId, name, data);
                }
                else
                {
                    add(userId, name, data);
                }

                return true;
            }

        }
        public bool save(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string name = p.GetParamValue("name");
            string data = p.GetParamValue("data");

            return save(epriseId, name, data);
        }

    }
}
