using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using InfoTrendsCommon.OAuth;
using InfoTrendsAPI.Type.Sms;
using CommonLibrary.DataContext;
using InfoTrendsCommon.DataContext;
using CommonLibrary.Extensions;
using CommonLibrary.Utility;
using InfoTrendsCommon.Exceptions;
using InfoTrendsModel.SmsModels;
using Newtonsoft.Json;
using System.Configuration;

namespace InfoTrendsAPI.Methods.Sms
{
    public class Folder
    {
        /// <summary>
        /// Add new folder
        /// </summary>
        public Int64 add(Guid contactId, int epriseId, string name, int? parentId, FolderType type)
        {
            if (parentId > 0)
            {
                if (get((Int64)parentId) == null)
                {
                    throw new MethodException("ParentId does not exist");
                }
            }


            string sqlStr = @"
                INSERT INTO {TableName} 
                                ( folderName,  contactId,  userId,  folderType,  parentFolderId )
                        VALUES  (@folderName, @contactId, @userId, @folderType, {ParentFolderId})
                        {ScopeIdentity}
            ";
            

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedFolders)
                .Replace("{ParentFolderId}", parentId > 0 ? parentId.ToString() : "NULL")
                .Replace("{ScopeIdentity}", SqlStatement.SQL_GET_SCOPE_IDENTITY)
                ;


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                // Parameters
                sqlCmd.Parameters.Add(new SqlParameter("@folderName", name));
                sqlCmd.Parameters.Add(new SqlParameter("@contactId", contactId));
                sqlCmd.Parameters.Add(new SqlParameter("@userId", epriseId));
                sqlCmd.Parameters.Add(new SqlParameter("@folderType", (int)type));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                    {
                        throw new MethodException("Error creating Folder");
                    }

                    Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());

                    // Return
                    return newID;
                }
            }
        }
        public Int64 add(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;
            string name = p.GetParamValue("name");
            string parentIdStr = p.GetParamValue("parent_id");
            string typeStr = p.GetParamValue("type");

            if (name.IsNullOrWhiteSpace())
            {
                throw new MethodException("Invalid folder name");
            }


            int? parentId;
            try { parentId = int.Parse(parentIdStr); }
            catch { parentId = null; }


            FolderType type;
            if (!Enum.TryParse<FolderType>(typeStr, true, out type))
            {
                throw new MethodException("Invalid type");
            }


            // Return
            return add(contactId, epriseId, name, parentId, type);
        }


        /// <summary>
        /// Get folder
        /// </summary>
        public FolderModel get(Int64 folderId)
        {
            #region string sqlStr = @"..."
            string sqlStr = @"
                SELECT * 
                FROM {TableName}
                WHERE folderId={FolderId}
            ";
            #endregion


            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedFolders)
                .Replace("{FolderId}", folderId.ToString())
                ;


            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                {
                    return null;
                }

                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<FolderModel>>(jsonStr);
                return ret[0];
            }

        }
        public FolderModel get(InfoTrendsContext p)
        {
            Guid contactId = (Guid)p.User.crmContactGUID;
            string folderIdStr = p.GetParamValue("folder_id");

            Int64 folderId;
            if (!Int64.TryParse(folderIdStr, out folderId))
            {
                throw new MethodException("Invalid folder id");
            }

            // Validate
            FolderModel f = get(folderId);
            if (f != null && f.ContactId != contactId)
            {
                throw new MethodException("Access Denied");
            }


            // Return
            return f;
        }


        /// <summary>
        /// List folders
        /// </summary>
        public List<FolderModel> list(Guid contactId, FolderType type)
        {
            #region string sqlStr = @"..."
            string sqlStr = @"
            WITH folderList AS
            (
		            SELECT 
			            folderId,
			            folderName,
			            parentFolderId, 
			            userId, 
			            contactId, 
			            folderType, 
			            1 AS folderLevel 
		            FROM {TableName} AS tf
		            WHERE
			            tf.parentFolderId IS NULL
			
	            UNION ALL

		            SELECT 
			            child.folderId,
			            child.folderName,
			            child.parentFolderId,
			            child.userId, 
			            child.contactId, 
			            child.folderType, 
			            folderLevel + 1 AS folderLevel 
		            FROM {TableName} AS child
			            INNER JOIN folderList fl ON child.parentFolderId = fl.folderId
		            WHERE
			            child.parentFolderId IS NOT NULL
            )

            SELECT * FROM folderList
            WHERE 
                contactId = '{ContactId}' 
                AND folderType = {FolderType}
            ORDER BY
                folderLevel ASC,
                folderName ASC

            ";
            #endregion


            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedFolders)
                .Replace("{ContactId}", contactId.ToString())
                .Replace("{FolderType}", ((int)type).ToString())
                ;


            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                if (dt.Rows.Count == 0)
                {
                    return new List<FolderModel>();
                }

                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<FolderModel>>(jsonStr);
                return ret;
            }

        }
        public List<FolderModel> list(InfoTrendsContext p)
        {
            Guid contactId = (Guid)p.User.crmContactGUID;
            string typeStr = p.GetParamValue("type");

            FolderType type;
            if (!Enum.TryParse<FolderType>(typeStr, true, out type))
            {
                throw new MethodException("Invalid type");
            }

            // Return
            return list(contactId, type);
        }


        /// <summary>
        /// Remove existing folder
        /// </summary>
        public bool remove(Int64 folderId)
        {
            string sqlStr = DbConstant.SmsDb.StoreProcedure.DeleteSavedFolder;
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@folderId", folderId));
                SqlServerUtil.ExecCommand(sqlCmd);
                return true;
            }
        }
        public bool remove(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;
            string folderIdStr = p.GetParamValue("folder_id");

            Int64 folderId;
            try { folderId = Int64.Parse(folderIdStr); }
            catch { throw new MethodException("Invalid folder id"); }
            
            // Return
            return remove(folderId);
        }


        /// <summary>
        /// Update existing folder
        /// </summary>
        public bool update(Guid contactId, Int64 folderId, string name, int? parentId)
        {
            string sqlStr = @"
                UPDATE {TableName} SET 
                    folderName=@folderName,
                    parentFolderId={ParentFolderId}
                WHERE
                    folderId=@folderId
                    AND contactId=@contactId'
            ";


            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.SavedFolders)
                .Replace("{ParentFolderId}", parentId > 0 ? parentId.ToString() : "NULL")
                ;


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                // Parameters
                sqlCmd.Parameters.Add(new SqlParameter("@folderName", name));
                sqlCmd.Parameters.Add(new SqlParameter("@folderId", folderId));
                sqlCmd.Parameters.Add(new SqlParameter("@contactId", contactId));
                
                // Execute
                SqlServerUtil.ExecCommand(sqlCmd);
                return true;
            }
        }
        public bool update(InfoTrendsContext p)
        {
            Guid contactId = (Guid)p.User.crmContactGUID;
            string folderIdStr = p.GetParamValue("folder_id");
            string name = p.GetParamValue("name");
            string parentIdStr = p.GetParamValue("parent_id");


            if (name.IsNullOrWhiteSpace())
            {
                throw new MethodException("Invalid folder name");
            }


            Int64 folderId;
            try { folderId = Int64.Parse(folderIdStr); }
            catch { throw new MethodException("Invalid folder id"); }


            int? parentId;
            try { parentId = int.Parse(parentIdStr); }
            catch { parentId = null; }


            // Return
            return update(contactId, folderId, name, parentId);
        }


    }
}
