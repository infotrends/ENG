using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CommonLibrary.DataContext;
using CommonLibrary.Utility;
using CommonLibrary.Extensions;

using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using System.Configuration;

namespace InfoTrendsAPI.Methods.Sms
{
    public class UserInfo
    {


        /// <summary>
        /// Get user info
        /// </summary>
        public InfoTrendsModel.SmsModels.UserInfo get(int epriseId)
        {
            string sqlStr = "SELECT * FROM {0} WHERE epriseID={1}";

            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, epriseId);

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                    return null;

                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.UserInfo>>(jsonStr);
                return ret[0];
            }
        }
        public InfoTrendsModel.SmsModels.UserInfo get(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;

            // return
            return get(epriseId);
        }



        /// <summary>
        /// Get last login
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DateTime? getLastLogin(int epriseId)
        {
            var userInfo = get(epriseId);
            
            if (userInfo != null)
            {
                DateTime? lastLogin = userInfo.lastLogin;
                return lastLogin;
            }

            return null;
        }
        public DateTime? getLastLogin(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;

            // return
            return getLastLogin(epriseId);
        }



        /// <summary>
        /// Set agreement
        /// </summary>
        public bool setAgreement(int epriseId, bool doAccept)
        {
            try
            {
                string sqlStr = "";
                if (this.Exists(epriseId))
                {
                    sqlStr = "UPDATE {0} SET acceptAgreement='{1}' WHERE epriseID={2}";
                    sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, doAccept, epriseId);
                }
                else
                {
                    sqlStr = "INSERT INTO {0} (acceptAgreement, epriseID) VALUES ('{1}', {2})";
                    sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, doAccept, epriseId);
                }

                //execute
                SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);

                // return
                return true;
            }
            catch
            {
                // return
                return false;
            }
        }
        public bool setAgreement(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;
            bool doAccept = bool.Parse(p.GetParamValue("data"));

            // return
            return setAgreement(epriseId, doAccept);
        }



        /// <summary>
        /// Set last login
        /// </summary>
        public bool setLastLogin(int epriseId)
        {
            try
            {
                string lastLogin = DateTime.Now.ToString();
                string sqlStr = "";
                if (this.Exists(epriseId))
                {
                    sqlStr  = " UPDATE {0} SET lastLogin='{1}' WHERE epriseID={2}";
                    sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, lastLogin, epriseId);
                }
                else
                {
                    sqlStr = "INSERT INTO {0} (lastLogin, epriseID) VALUES ('{1}', {2})";
                    sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, lastLogin, epriseId);
                }

                //execute
                SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);

                // return
                return true;
            }
            catch
            {
                // return
                return false;
            }
        }
        public bool setLastLogin(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;

            // return
            return setLastLogin(epriseId);
        }


        /// <summary>
        /// Set report filter string
        /// ** because the component of flexmonster provided does not allow POST method to retrieve data. 
        /// ** Since the GET method will be overflowed because the filterString is too long.
        /// ** To solve this problem, first save the filterString of the report to this table using POST
        /// ** then retrieve it by providing the flexmonster a shorter GET url.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public bool setReportFilter(Int32 userId, string filterString)
        {
            try
            {
                string sqlStrUpdate = @" UPDATE {0} SET reportFilter=@filterString WHERE epriseID=@epriseId ";
                string sqlStrInsert = @" INSERT INTO {0} (reportFilter, epriseID) VALUES (@filterString, @epriseId) ";
                
                // decide
                string sqlStr = Exists(userId) ? sqlStrUpdate : sqlStrInsert;

                // format
                sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo);

                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
                {
                    // add parameters
                    sqlCmd.Parameters.Add(new SqlParameter("@filterString", filterString));
                    sqlCmd.Parameters.Add(new SqlParameter("@epriseId", userId));

                    // execute
                    SqlServerUtil.ExecCommand(sqlCmd);

                    // return
                    return true;
                }
            }
            catch
            {
                // return
                return false;
            }
        }
        public bool setReportFilter(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string filterString = p.GetParamValue("data");

            // return
            return setReportFilter(epriseId, filterString);
        }



        #region UTILITY



        /// <summary>
        /// exists record
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Boolean Exists(int epriseId)
        {
            string sqlStr = "SELECT COUNT(epriseID) FROM {0} WHERE epriseID={1}";
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.UserInfo, epriseId);

            // execute
            object result = SqlServerUtil.ExecScalar(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);
            int count = (int)result;
            
            // return
            return (count > 0);
        }




        #endregion




    }
}
