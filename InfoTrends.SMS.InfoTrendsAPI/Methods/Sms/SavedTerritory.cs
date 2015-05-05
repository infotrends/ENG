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
    public class SavedTerritory
    {

        /// <summary>
        /// Create SavedTerritory
        /// </summary>
        public Int64 add(Guid contactId, int userId, Int64? folderId, string name, string mapInfo, string boundaryType, string color, string note, string[] tileIds)
        {
            string sqlStr = @"
                INSERT INTO {TableName}
                            ( contactId,  epriseID,  folderId,  name,  mapInfo,  boundaryType,  color,  note) 
                    VALUES  (@contactId, @epriseId, @folderId, @name, @mapInfo, @boundaryType, @color, @note) 
                    {ScopeIdentity}
            ";

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.Territories)
                .Replace("{ScopeIdentity}", SqlStatement.SQL_GET_SCOPE_IDENTITY)
                ;


            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@contactId", contactId));
                sqlCmd.Parameters.Add(new SqlParameter("@epriseId", userId));
                sqlCmd.Parameters.Add(new SqlParameter("@folderId", folderId));
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@mapInfo", mapInfo));
                sqlCmd.Parameters.Add(new SqlParameter("@boundaryType", boundaryType));
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));

                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt == null)
                    {
                        throw new MethodException(SmsError.TERRITORY_COULD_NOT_BE_CREATED);
                    }

                    // Create
                    Int64 newID = Int64.Parse(dt.Rows[0][0].ToString());

                    // add territory tile
                    AddTerritoryTile(newID, tileIds);

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
            string mapInfo = p.GetParamValue("map_info");
            string boundaryType = p.GetParamValue("boundary_type");
            string color = p.GetParamValue("color");
            string note = p.GetParamValue("note");
            string tileIdsStr = p.GetParamValue("tile_ids");

            Int64? folderId;
            try { folderId = Int64.Parse(folderIdStr); }
            catch { folderId = null; }

            string[] tileIds = tileIdsStr.Split(',');

            return add(contactId, epriseId, folderId, name, mapInfo, boundaryType, color, note, tileIds);
        }


        /// <summary>
        /// Delete territory
        /// NOTE: check if territoryGrid items that belong to this territory should be deleted as well
        /// FOREIGN KEY is in used.
        /// </summary>
        /// <returns></returns>
        public bool delete(Int64 territoryId)
        {
            string sqlStr = @"
                DELETE FROM {TableName} 
                WHERE territoryId={TerritoryId}
            ";

            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.Territories)
                .Replace("{TerritoryId}", territoryId.ToString())
                ;

            SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);
            return true;
        }
        //public bool delete(InfoTrendsContext p)
        //{
        //    Guid contactId = (Guid)p.User.crmContactGUID;
        //    string recIdStr = p.GetParamValue("rec_id");

        //    Int64 recId;
        //    if (!Int64.TryParse(recIdStr, out recId))
        //    {
        //        throw new MethodException("Invalid Record Id");
        //    }

        //    return delete(recId);
        //}


        /// <summary>
        /// Territory aggregation data (legacy)
        /// </summary>
        /// <returns>DataTable</returns>
        public InfoTrendsModel.SmsModels.TerritoryAggregation getAggregationData(Int64 territoryId, string boundaryType, string filterString)
        {
            boundaryType = SqlStatement.Normalize(boundaryType);
            filterString = SqlStatement.Normalize(filterString);
            filterString = filterString.Replace("''", "'");

            string sqlStr = "sp_territoryAggregationData";
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                // set command type
                sqlCmd.CommandType = CommandType.StoredProcedure;

                // set command parameters
                sqlCmd.Parameters.Add(new SqlParameter("@territoryID", territoryId));
                sqlCmd.Parameters.Add(new SqlParameter("@territoryType", boundaryType));
                sqlCmd.Parameters.Add(new SqlParameter("@filterString", filterString));

                // execute
                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt.Rows.Count == 0)
                        return null;

                    string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.TerritoryAggregation>>(jsonStr);
                    return ret[0];
                }
            }

        }
        public InfoTrendsModel.SmsModels.TerritoryAggregation getAggregationData(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string boundaryType = p.GetParamValue("boundary_type");
            string filter = p.GetParamValue("filter");
            string recIdStr = p.GetParamValue("rec_id");
            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }



            return getAggregationData(recId, boundaryType, filter);
        }


        /// <summary>
        /// Territory aggregation data
        /// </summary>
        /// <returns>DataTable</returns>
        public List<InfoTrendsModel.SmsModels.TerritoryAggregationBpi> getAggregationDataBpi(Int64 territoryId, string boundaryType, string filterString)
        {
            boundaryType = SqlStatement.Normalize(boundaryType);
            filterString = SqlStatement.Normalize(filterString);
            filterString = filterString.Replace("''", "'");

            string sqlStr = "sp_ComputeTerritoryOverallBpiAverage";
            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                // set command type
                sqlCmd.CommandType = CommandType.StoredProcedure;

                // set command parameters
                sqlCmd.Parameters.Add(new SqlParameter("@TerritoryId", territoryId));
                //sqlCmd.Parameters.Add(new SqlParameter("@TerritoryType", boundaryType));
                sqlCmd.Parameters.Add(new SqlParameter("@FilterString", filterString));

                // execute
                using (DataTable dt = SqlServerUtil.GetData(sqlCmd))
                {
                    if (dt.Rows.Count == 0)
                        return null;

                    string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                    var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.TerritoryAggregationBpi>>(jsonStr);
                    return ret;
                }
            }

        }
        public List<InfoTrendsModel.SmsModels.TerritoryAggregationBpi> getAggregationDataBpi(InfoTrendsContext context)
        {
            var p = context;
            int epriseId;

            try
            {
                epriseId = (int)p.User.epriseId;
            }
            catch
            {
                throw new MethodException("Access Denied");
            }


            string boundaryType = p.GetParamValue("boundary_type");
            string filter = p.GetParamValue("filter");
            string recIdStr = p.GetParamValue("rec_id");
            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }



            return getAggregationDataBpi(recId, boundaryType, filter);
        }



        /// <summary>
        /// List SavedTerritories
        /// </summary>
        public List<InfoTrendsModel.SmsModels.SavedTerritory> list(int userId)
        {
            string sqlStr = @"
                SELECT * 
                FROM {TableName} 
                WHERE epriseId={EpriseId} 
                ORDER BY dateCreated DESC
            ";

            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.Territories)
                .Replace("{EpriseId}", userId.ToString())
                ;

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedTerritory>>(jsonStr);
                return ret;
            }
        }

        public InfoTrendsModel.SmsModels.SavedTerritory GetById(long? id)
        {
            //boundaryType, color, dateCreated, epriseID, name, note 
            string sqlStr = @"
                SELECT *
                FROM {TableName} 
                WHERE territoryID={id}
            ";

            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.Territories)
                .Replace("{id}", id.ToString())
                ;

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedTerritory>>(jsonStr);
                return ret.FirstOrDefault();
            }
        }
        //public List<InfoTrendsModel.SmsModels.SavedTerritory> list(InfoTrendsContext p)
        //{
        //    Guid contactId = (Guid)p.User.crmContactGUID;
        //    int epriseId = (int)p.User.epriseId;
        //    return list(contactId, epriseId);
        //}


        /// <summary>
        /// Update existing SavedTerritory
        /// </summary>
        public bool update(long? recId, string name, string mapInfo, string color, string note)
        {
            string mapInfoStatement = (mapInfo == "") ? "" : " , mapInfo=@mapInfo ";

            string sqlStr = @"
                UPDATE {TableName} SET                    
                    name=@name, 
                    color=@color, 
                    note=@note
                    {Extra} 
                WHERE 
                    territoryID={TerritoryId}
            ";

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.Table.Territories)
                .Replace("{TerritoryId}", recId.ToString())
                .Replace("{Extra}", mapInfoStatement)
                ;

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                sqlCmd.Parameters.Add(new SqlParameter("@name", name));
                sqlCmd.Parameters.Add(new SqlParameter("@color", color));
                sqlCmd.Parameters.Add(new SqlParameter("@note", note));

                if (mapInfoStatement.Length > 0)
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@mapInfo", mapInfo));
                }

                SqlServerUtil.ExecCommand(sqlCmd);

                return true;
            }
        }
        //public bool update(InfoTrendsContext p)
        //{
        //    int epriseId = (int)p.User.epriseId;
        //    Guid contactId = (Guid)p.User.crmContactGUID;

        //    string folderIdStr = p.GetParamValue("folder_id");
        //    string name = p.GetParamValue("name");
        //    string mapInfo = p.GetParamValue("map_info");
        //    string color = p.GetParamValue("color");
        //    string note = p.GetParamValue("note");
        //    string recIdStr = p.GetParamValue("rec_id");

        //    Int64 recId;
        //    if (!Int64.TryParse(recIdStr, out recId))
        //    {
        //        throw new MethodException("Invalid Record Id");
        //    }

        //    Int64? folderId;
        //    try { folderId = Int64.Parse(folderIdStr); }
        //    catch { folderId = null; }

        //    // Return
        //    return update(contactId, epriseId, recId, folderId, name, mapInfo, color, note);
        //}



        /// <summary>
        /// Return all saved territories of the specific user
        /// </summary>
        public List<InfoTrendsModel.SmsModels.SavedTerritoryTile> listTile(Int64 territoryId)
        {
            string sqlStr = @"
                SELECT * 
                FROM {TableName} 
                WHERE territoryId={TerritoryId}
            ";

            // Format
            sqlStr = sqlStr
                .Replace("{TableName}", DbConstant.SmsDb.View.SavedTerritoriesView)
                .Replace("{TerritoryId}", territoryId.ToString())
                ;

            using (DataTable dt = SqlServerUtil.GetData(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString))
            {
                string jsonStr = JsonConvert.SerializeObject(dt);
                var ret = JsonConvert.DeserializeObject<List<InfoTrendsModel.SmsModels.SavedTerritoryTile>>(jsonStr);
                return ret;
            }
        }
        public List<InfoTrendsModel.SmsModels.SavedTerritoryTile> listTile(InfoTrendsContext p)
        {
            int epriseId = (int)p.User.epriseId;
            Guid contactId = (Guid)p.User.crmContactGUID;
            string recIdStr = p.GetParamValue("rec_id");

            Int64 recId;
            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            // Return
            return listTile(recId);
        }


        /// <summary>
        /// Update saved territory of specific user
        /// </summary>
        public bool updateTile(int userId, Int64 recId, string[] tileIds)
        {
            string sqlStr = "";

            // check exist of user
            sqlStr = "SELECT COUNT(*) FROM {0} WHERE epriseID={1}".Fmt(DbConstant.SmsDb.Table.Territories, userId);
            int count = (int)SqlServerUtil.ExecScalar(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);
            if (count == 0) throw new MethodException(SmsError.TERRITORY_COULD_NOT_BE_UPDATED);


            // first: remove the old territory grids
            sqlStr = "DELETE FROM {0} WHERE territoryID={1}".Fmt(DbConstant.SmsDb.Table.TerritoryGrid, recId);
            SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);


            // second: create new set territory grid
            AddTerritoryTile(recId, tileIds);
            

            // return success
            return true;
        }
        public bool updateTile(InfoTrendsContext context)
        {
            var p = context;
            int epriseId = (int)p.User.epriseId;
            string tileIdsStr = p.GetParamValue("tile_ids");
            string recIdStr = p.GetParamValue("rec_id");

            Int64 recId;

            if (!Int64.TryParse(recIdStr, out recId))
            {
                throw new MethodException("Invalid Record Id");
            }

            string[] tileIds = tileIdsStr.Split(',');

            return updateTile(epriseId, recId, tileIds);
        }



        #region# UTILITY

        /// <summary>
        /// Create territory tile
        /// </summary>
        protected bool AddTerritoryTile(Int64 territoryId, string[] tileIds)
        {
            string sqlStr = "";

            foreach (string tileId in tileIds)
            {
                sqlStr += " INSERT INTO {0} (territoryID, boundaryID) VALUES ({1}, {2});"
                    .Fmt(DbConstant.SmsDb.Table.TerritoryGrid, territoryId, tileId);
            }

            SqlServerUtil.ExecCommand(sqlStr, ConfigurationManager.ConnectionStrings["InfoTrendsSMS"].ConnectionString);

            return true;
        }


        #endregion


    }
}
