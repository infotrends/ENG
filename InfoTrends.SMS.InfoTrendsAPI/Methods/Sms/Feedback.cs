using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CommonLibrary.Extensions;
using CommonLibrary.Utility;
using CommonLibrary.DataContext;

using InfoTrendsAPI.Error;

using InfoTrendsCommon.DataContext;
using InfoTrendsCommon.OAuth;
using System.Configuration;


namespace InfoTrendsAPI.Methods.Sms
{
    public class Feedback
    {


        /// <summary>
        /// Add new feedback
        /// </summary>
        /// <param name="epriseID"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public Int64 add(int epriseId, string note)
        {
            try
            {
                string sqlStr = "INSERT INTO {0} (epriseId,  note) VALUES ({1}, @note)";
                sqlStr += SqlStatement.SQL_GET_SCOPE_IDENTITY;

                // format
                sqlStr = sqlStr.Fmt(DbConstant.SmsDb.Table.Feedback, epriseId);


                // command
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
                {

                    // add parameters
                    sqlCmd.Parameters.Add(new SqlParameter("@note", note));
                

                    // execute
                    using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                    {

                        if (dt == null)
                            throw new Exception(SmsError.FEEDBACK_COULD_NOT_BE_CREATED);

                        // new id
                        Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());

                        // return
                        return newID;
                    }
                }

            }
            catch (Exception e)
            {
                string msg = OAuthError.INVALID_METHOD_SIGNATURE;
                if (e.Message == SmsError.FEEDBACK_COULD_NOT_BE_CREATED)
                    msg = e.Message;

                throw new Exception(msg);
            }
        }
        public Int64 add(InfoTrendsContext context)
        {
            // parameters
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string note = p.GetParamValue("note");

            // return
            return add(epriseId, note);
        }



    }
}
