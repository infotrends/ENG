using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitebracoApi.UmbracoDoctypes;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace SitebracoApi
{
    public static class IMemberServiceExtensions
    {
        /// <summary>
        /// Find member by property
        /// </summary>
        /// <param name="propertyName">Name of property to find</param>
        /// <param name="value">Value of property to find</param>
        /// <returns></returns>
        public static List<IMember> FindByProperty(this IMemberService value, string propertyName, string keyword)
        {
            #region string sqlStr = @"..."
            string sqlStr = @"
                SELECT 
	                MEMBTYPES.Alias AS MemberFieldAlias,  
                    ISNULL(CASE 
		                WHEN MEMBTYPES.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Ntext') THEN MEMBDATA.[dataNtext] 
		                WHEN MEMBTYPES.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Nvarchar') THEN MEMBDATA.[dataNvarchar] 
		                WHEN MEMBTYPES.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Date') THEN CONVERT(NVARCHAR, MEMBDATA.[dataDate]) 
		                WHEN MEMBTYPES.datatypeID IN (SELECT NodeId FROM DBO.CMSDATATYPE WHERE DBTYPE = 'Integer') THEN CONVERT(NVARCHAR, MEMBDATA.[dataInt]) 
		                ELSE NULL END, NULL) AS MemberData, 
                    MEMB.LoginName, 
                    MEMB.Email, 
                    MEMB.nodeId, 
                    MEMBNODE.text as MemberName
    
                FROM 
                    (SELECT id, text, createDate, nodeUser FROM dbo.umbracoNode WHERE (nodeObjectType = '9b5416fb-e72f-45a9-a07b-5a9a2709ce43')) AS MEMBTYPEID 
                    LEFT OUTER JOIN (SELECT nodeId, contentType FROM dbo.cmsContent) AS MEMBLST ON MEMBLST.contentType = MEMBTYPEID.id 
                    LEFT OUTER JOIN dbo.cmsPropertyType AS MEMBTYPES ON MEMBTYPES.contentTypeId = MEMBLST.contentType 
                    LEFT OUTER JOIN dbo.cmsPropertyData AS MEMBDATA ON MEMBDATA.contentNodeId = MEMBLST.nodeId AND MEMBDATA.propertytypeid = MEMBTYPES.id 
                    LEFT OUTER JOIN dbo.cmsMember AS MEMB ON MEMB.nodeId = MEMBLST.nodeId
                    LEFT OUTER JOIN dbo.umbracoNode AS MEMBNODE ON MEMBNODE.id = MEMB.nodeId
    
                WHERE 
	                MEMBTYPES.Alias = @propertyName AND MEMBDATA.[dataNtext] like @search
                    OR MEMBTYPES.Alias = @propertyName AND MEMBDATA.[dataNvarchar] like @search
                    OR MEMBTYPES.Alias = @propertyName AND MEMBDATA.[dataDate] like @search
                    OR MEMBTYPES.Alias = @propertyName AND MEMBDATA.[dataInt] like @search
    
                ORDER BY 
	                MEMB.nodeId DESC
            ";
            #endregion


            using (SqlConnection sqlConn = new SqlConnection(Constant.UmbracoConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
            {
                // Parameters
                sqlCmd.Parameters.Add(new SqlParameter("@propertyName", propertyName));
                //sqlCmd.Parameters.Add(new SqlParameter("@search", "%" + keyword + "%"));
                sqlCmd.Parameters.Add(new SqlParameter("@search", keyword));

                using (DataTable results = SqlServerUtil.GetData(sqlCmd))
                {
                    var ret = new List<IMember>();
                    foreach (DataRow dr in results.Rows)
                    {
                        var memberId = (int)dr["nodeId"];
                        var iMember = ApplicationContext.Current.Services.MemberService.GetById(memberId);
                        if (iMember != null)
                        {
                            ret.Add(iMember);
                        }
                    }
                    return ret;
                }
            }
        }


        /// <summary>
        /// Get member by contact id
        /// 
        /// Exception:
        ///     Exception will be thrown if duplicate contacts found
        /// </summary>
        /// <param name="contactId">Contact id to find</param>
        /// <returns>IMember</returns>
        public static IMember GetByContactId(this IMemberService value, Guid contactId)
        {
            return GetByContactId(value, contactId.ToString());
        }


        /// <summary>
        /// Get member by contact id string
        /// 
        /// Exception:
        ///     Exception will be thrown if duplicate contacts found
        /// </summary>
        /// <param name="contactId">Contact id string to find</param>
        /// <returns></returns>
        public static IMember GetByContactId(this IMemberService value, string contactId)
        {
            var results = FindByProperty(
                value,
                ObjectUtil.GetPropertyName<UmbracoDoctypes.Member>(x => x.member_contactId),
                contactId
                );

            if (results.Count > 1)
            {
                throw new Exception("Duplicate contacts");
            }
            else if (results.Count == 1)
            {
                return results[0];
            }

            return null;
        }

    }

}
