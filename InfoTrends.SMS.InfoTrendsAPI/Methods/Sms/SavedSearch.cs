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
    public class SavedSearch
    {
        /// <summary>
        /// Create SavedSearch
        /// </summary>
        public Int64 add(Guid contactId, int userId, Int64? folderId, string name, string filter, string mapInfo, string color, string note)
        {

            string sqlStr = @"
            INSERT INTO {TableName}
                            ( contactId,  epriseId,  folderId,  name,  filter,  mapInfo,  color,  note)
                    VALUES  (@contactId, @epriseId, @folderId, @name, @filter, @mapInfo, @color, @note)
                    {ScopeIdentity}
            ";
                        
            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedSearches)
                .Replace("{ScopeIdentity}", SqlStatement.SQL_GET_SCOPE_IDENTITY)
                ;


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@contactId", contactId));
                sqlCmd.Parameters.Add(new SqlParameter("@epriseId", userId));
                sqlCmd.Parameters.Add(new SqlParameter("@folderId", folderId));
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@filter", filter));
                sqlCmd.Parameters.Add(new SqlParameter("@mapInfo", mapInfo));
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                    {
                        throw new MethodException(SmsError.SEARCHES_COULD_NOT_BE_CREATED);
                    }


                    Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());
                    return newID;
                }
            }
        }
        public Int64 add(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;
            string folderIdStr = p.GetParamValue("folder_id");
            string name = p.GetParamValue("name");
            string filter = p.GetParamValue("filter");
            string mapInfo = p.GetParamValue("map_info");
            string color = p.GetParamValue("color");
            string note = p.GetParamValue("note");

            Int64? folderId;
            try { folderId = Int64.Parse(folderIdStr); }
            catch { folderId = null; }

            // Return
            return add(contactId, epriseId, folderId, name, filter, mapInfo, color, note);
        }


        /// <summary>
        /// Delete existing SavedSearch
        /// </summary>
        public bool delete(Guid contactId, int userId, Int64 recId)
        {
            string sqlStr = @"
                DELETE FROM {TableName}
                WHERE 
                    epriseID={EpriseId} 
                    AND ID={Id}
            ";

            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedSearches)
                .Replace("{EpriseId}", userId.ToString())
                .Replace("{Id}", recId.ToString())
                ;
            
            SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);

            return true;
        }
        public bool delete(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;

            string recIdStr = p.GetParamValue("rec_id");
            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            return delete(contactId, epriseId, recId);
        }


        /// <summary>
        /// List SavedSearches
        /// </summary>
        public List<InfoTrendsModel.SmsModels.SavedSearch> list(Guid contactId, int userId)
        {
            string sqlStr = @"
                SELECT * 
                FROM {TableName} 
                WHERE epriseID={EpriseId} 
                ORDER BY dateCreated DESC 
            ";

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedSearches)
                .Replace("{EpriseId}", userId.ToString())
                ;

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedSearch>>(jsonStr);
                return ret;
            }
        }

        public InfoTrendsModel.SmsModels.SavedSearch GetById(long? ID)
        {
            string sqlStr = @"
                SELECT * 
                FROM {TableName} 
                WHERE ID={ID} 
                
            ";

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedSearches)
                .Replace("{ID}", ID.ToString())
                ;

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedSearch>>(jsonStr);
                return ret.ToList().FirstOrDefault();
            }
        }

        public List<InfoTrendsModel.SmsModels.SavedSearch> list(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;
            return list(contactId, epriseId);
        }


        /// <summary>
        /// Update existing SavedSearch
        /// </summary>
        public bool update(int userId, Int64 recId, string name, string mapInfo, string color, string visible)
        {
            string sqlStr = @"
                UPDATE {TableName} SET                    
                    name=@name, 
                    mapInfo=@mapInfo, 
                    color=@color, 
                    visible=@visible 
                WHERE 
                    epriseID={EpriseId} 
                    AND ID={Id}
            ";

            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedSearches)
                .Replace("{EpriseId}", userId.ToString())
                .Replace("{Id}", recId.ToString())
                ;


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@mapInfo", mapInfo));
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@visible", visible));

                SqlServerUtil.ExecCommand(sqlCmd);

                return true;
            }

        }
    }
}
