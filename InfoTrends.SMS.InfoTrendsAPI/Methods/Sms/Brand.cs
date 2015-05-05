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
    public class Brand
    {

        /// <summary>
        /// Listing all brands
        /// </summary>
        /// <returns></returns>
        public List<InfoTrendsModel.SmsModels.Brand> list()
        {
            string sqlStr = @"
                SELECT DISTINCT companyName, companyID 
                FROM {0} 
                WHERE companyID > 0 
                ORDER BY companyName ASC
            ";

            // format
            sqlStr = sqlStr.Fmt(DbConstant.SmsDb.View.SmsGrid);

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.Brand>>(jsonStr);
                return ret;
            }

        }
        public List<InfoTrendsModel.SmsModels.Brand> list(InfoTrendsContext context)
        {
            // return
            return list();
        }



    }
}
