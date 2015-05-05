using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CommonLibrary.Extensions;
using CommonLibrary.Utility;

using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using System.Configuration;



namespace InfoTrendsAPI.Methods.Sms
{
    public class ApplicationHistory
    {

        /// <summary>
        /// List all application features
        /// </summary>
        /// <returns></returns>
        public List<InfoTrendsModel.SmsModels.ApplicationHistory> listAllFeatures()
        {
            int? epriseId = null;
            return listNewFeatures(epriseId);
        }
        public List<InfoTrendsModel.SmsModels.ApplicationHistory> listAllFeatures(InfoTrendsContext context)
        {
            // return
            return listAllFeatures();
        }




        /// <summary>
        /// List new application features since the last login of user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<InfoTrendsModel.SmsModels.ApplicationHistory> listNewFeatures(int? epriseId)
        {
            DateTime? lastLogin = null;
            if (epriseId != null)
            {
                UserInfo userInfo = new UserInfo();
                lastLogin = userInfo.getLastLogin((int)epriseId);
                userInfo = null;
            }
            
            // result
            var ret = ListNewFeatures(lastLogin);

            // return
            return ret;
        }
        public List<InfoTrendsModel.SmsModels.ApplicationHistory> listNewFeatures(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;

            // return
            return listNewFeatures(epriseId);
        }





        #region " UTILITY "



        /// <summary>
        /// List new application features
        /// </summary>
        /// <param name="lastLogin"></param>
        /// <returns></returns>
        List<InfoTrendsModel.SmsModels.ApplicationHistory> ListNewFeatures(DateTime? lastLogin)
        {
            string sqlStr = @"
                SELECT *
                FROM {0}
                {1}
                ORDER BY 
                    category DESC, 
                    created DESC
            ";

            string sqlCondition = (lastLogin == null) ? "" : "WHERE created > '{1}'".Fmt(lastLogin);

            
            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.ApplicationHistory, sqlCondition);


            // get data
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.ApplicationHistory>>(jsonStr);
                return ret;
            }

        }



        #endregion





    }
}
