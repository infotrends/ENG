using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using SitebracoApi.DbEntities;
using SitebracoApi.Models;
using SitebracoApi.Models.ContentEditor;
using SitebracoApi.Services;
using SitebracoApi.Types;

using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.NodeFactory;
using System.Data.SqlClient;
using System.Data;
using MyUtils.Validations;


namespace SitebracoApi.Controllers.ContentEditor
{

    [PluginController(Constant.SitebracoApiModule.ContentEditor)]
    [Umbraco.Web.WebApi.MemberAuthorize(AllowGroup = Constant.SitebracoBasicRole.Employee)]
    public class ContentController : BaseController
    {

        [HttpGet]
        public ContentRetModel Get([FromUri]GetParam p)
        {
            try
            {
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                var ret = ContentRetModel.TransferSingle(content);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class GetParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> Publish([FromBody]PublishParam p)
        {
            try
            {
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Publish
                var result = Services.ContentService.SaveAndPublishWithStatus(content);
                return new SimpleObjectModel<bool>(result.Success);
            }
            catch (Exception e)
            {
                var error = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(error);
            }
        }
        public class PublishParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> PublishDeep([FromBody]PublishDeepParam p)
        {
            try
            {
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Save
                Services.ContentService.PublishWithChildrenWithStatus(content, 0, true);

                // Return
                var ret = new SimpleObjectModel<bool>(true);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class PublishDeepParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }



        [HttpPost]
        public SimpleObjectModel<bool> SetSourceFileType([FromBody]SetSourceFileParam p)
        {
            try
            {
                ValidateParams(p);


                var contentId = (int)p.ContentId;
                var sourceFileType = (ContentEditorTemplateType)p.SourceFileType;
                var sourceFileTemplateId = p.SourceFileTemplateId;


                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);


                // Whether to use the template
                if (sourceFileType == ContentEditorTemplateType.Excel || sourceFileType == ContentEditorTemplateType.PowerPoint || sourceFileType == ContentEditorTemplateType.Word)
                {
                    // Validate
                    if (sourceFileTemplateId == Guid.Empty)
                        throw new Exception("Invalid @SourceFileTemplateId");

                    // Retrieve template binary
                    using (var db = new SitebracoEntities())
                    {
                        var template = db.new_OfficeTemplateVersion
                            .Where(x => x.OfficeTemplateId == sourceFileTemplateId && x.MarkDeleteOn == null)
                            .OrderByDescending(x => x.Version)
                            .ToList()[0]
                            ;

                        // Set template
                        content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_sourceFile), template.TemplateBase64String);
                    }
                }


                // Set source file type
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_sourceFileType), sourceFileType.ToString());


                // Save
                Services.ContentService.Save(content);


                // Return
                return new SimpleObjectModel<bool>(true);
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }


        }
        public class SetSourceFileParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }

            [Required]
            public ContentEditorTemplateType? SourceFileType { get; set; }
            
            public Guid? SourceFileTemplateId { get; set; }
        }


        [HttpGet]
        public PagingResult<ContentRetModel> Search([FromUri]SearchParam p)
        {
            try
            {
                ValidateParams(p);


                // Default
                var page = p.Page == null ? 1 : (int)p.Page;
                var limit = p.Limit == null ? 10 : (int)p.Limit;

                if (page < 1) page = 1;
                if (limit < 1 || limit > 1000) limit = 100;

                var ret = new PagingResult<ContentRetModel>
                {
                    Data = new List<ContentRetModel>(),
                    Limit = limit,
                    Page = page,
                    TotalCount = 0
                };

                var terms = p.Keyword;
                if (!terms.IsNullOrWhiteSpace())
                {
                    var totalCount = (int)0;
                    var skip = (page - 1) * limit;
                    var luceneQueryString = BuildRawQueryForTerms(terms);
                    var results = Extlib.SearchExamine(InfoTrendsExamineSearcherType.InternalContentSearcher, luceneQueryString, skip, limit, out totalCount);

                    // set
                    ret.TotalCount = totalCount;

                    foreach (var item in results)
                    {
                        var node = new Node(item.Id);
                        if (node == null) continue;
                        var o = ContentRetModel.TransferSingle(node);
                        ret.Data.Add(o);
                    }

                    return ret;
                }

                // Return
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }


        }
        public class SearchParam
        {
            [Required]
            public string Keyword{ get; set; }
            public int? Page { get; set; }
            public int? Limit { get; set; }
        }



        [HttpGet]
        public List<AuthorRetModel> SearchAuthors([FromUri]SearchAuthorsParam p)
        {
            var ret = new List<AuthorRetModel>();

            try
            {
                ValidateParams(p);

                var keywords = p.Query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                var members = Services.MemberService
                    .GetMembersInRole(UmbracoMemberType.Employee.ToString())
                    .OrderBy(x => x.Name)
                    ;

                foreach (Member m in members)
                {
                    // Do top 5 only
                    if (ret.Count == 5)
                        break;

                    foreach (var q in keywords)
                    {
                        if (m.Name.ToLower().Contains(q.Trim().ToLower()))
                        {
                            var sp = new List<string>(m.Name.Split(','));
                            sp.Reverse();
                            var name = sp.JoinString(" ").Trim();
                            ret.Add(new AuthorRetModel { Keyword = name, Description = m.Email });
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            // Return
            return ret;
        }
        public class SearchAuthorsParam
        {
            [Required]
            public string Query { get; set; }
        }


        [HttpGet]
        public List<KeywordRetModel> SearchKeywords([FromUri]SearchKeywordsParam p)
        {
            var ret = new List<KeywordRetModel>();

            try
            {
                ValidateParams(p);


                var sqlStr = @"
                    SELECT DISTINCT TOP 5
	                    T.tag
                    FROM cmsTags T
	                    INNER JOIN cmsTagRelationship TR ON TR.tagId = T.Id
	                    INNER JOIN cmsPropertyType PT ON PT.id = TR.propertyTypeId
                    WHERE 
	                    PT.Alias = @propertyName
                        AND {0}
                    ORDER BY 
                        T.tag
                ";


                var keywords = p.Query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var sqlConditions = new List<string>();
                var sqlConditionParams = new List<SqlParameter>();

                // Serialize conditions
                for (var i = 0; i < keywords.Length; i++)
                {
                    sqlConditions.Add("T.tag LIKE @Parameter{0}".Fmt(i));
                    sqlConditionParams.Add(new SqlParameter("@Parameter{0}".Fmt(i), "%{0}%".Fmt(keywords[i])));
                }


                // Format
                sqlStr = sqlStr.Fmt(sqlConditions.JoinString(" AND "));


                // Connect database
                using (SqlConnection sqlConn = new SqlConnection(Constant.UmbracoConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlConn))
                {
                    // Parameters
                    sqlCmd.Parameters.Add(new SqlParameter("@propertyName", ObjectUtil.GetPropertyName<UmbracoDoctypes.ContentBase>(x => x.contentBase_keywords)));
                    sqlCmd.Parameters.AddRange(sqlConditionParams.ToArray());

                    using (DataTable results = SqlServerUtil.GetData(sqlCmd))
                    {
                        foreach (DataRow dr in results.Rows)
                        {
                            var tag = dr[0].ToString();
                            ret.Add(new KeywordRetModel { Keyword = tag, Description = "" });
                        }
                    }
                }

            }
            catch (Exception e)
            {
            }


            return ret;
        }
        public class SearchKeywordsParam
        {
            [Required]
            public string Query { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> UnPublish([FromBody]UnPublishParam p)
        {
            try
            {
                ValidateParams(p);

                var contentId = (int)p.ContentId;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);

                // Save
                Services.ContentService.UnPublish(content);

                // Return
                var ret = new SimpleObjectModel<bool>(true);
                return ret;
            }
            catch (Exception e)
            {
                var msg = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(msg);
            }
        }
        public class UnPublishParam
        {
            [RequiredNumberGreaterThanZero]
            public int? ContentId { get; set; }
        }


        [HttpPost]
        public SimpleObjectModel<bool> Update([FromBody]UpdateParam p)
        {
            try
            {
                ValidateParams(p);


                var contentId = (int)p.Id;
                var content = Services.ContentService.GetById(contentId);
                Utility.Content.ValidateContentNode(content);


                // Abstract
                var abstractVal = p.Abstract.IsNullOrWhiteSpace() ? (object)"" : (object)p.Abstract;
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_abstract), abstractVal);


                // Authors
                var authorsVal = p.Authors == null ? (object)"" : (object)p.Authors.JoinString(",");
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_authors), authorsVal);


                // Available for purchase date
                object availableForPurchaseDateVal = p.AvailableForPurchaseDate == null ? (object)"" : (object)p.AvailableForPurchaseDate;
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_availableForPurchaseDate), availableForPurchaseDateVal);


                // Keywords
                var keywordsVal = p.Keywords == null ? (object)"" : (object)p.Keywords.JoinString(",");
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.contentBase_keywords), keywordsVal);


                // Price
                object priceVal = p.Price == null ? (object)"" : (object)p.Price;
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_pubDate), priceVal);


                // Pub date
                object pubDateVal = p.PubDate == null ? (object)"" : (object)p.PubDate;
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_pubDate), pubDateVal);


                // Title
                var titleVal = p.Title;
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_title), titleVal);


                // Class spin doctor
                var spinDoctorClassVal = ListToSpinDoctorValue(p.MetaClass);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_class), spinDoctorClassVal);


                // Industry spin doctor
                var spinDoctorIndustryVal = ListToSpinDoctorValue(p.MetaIndustry);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_industry), spinDoctorIndustryVal);


                // Market spin doctor
                var spinDoctorMarketVal = ListToSpinDoctorValue(p.MetaMarket);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_market), spinDoctorMarketVal);


                // Primary spin doctor
                var spinDoctorPrimaryVal = ListToSpinDoctorValue(p.MetaPrimary);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_primary), spinDoctorPrimaryVal);


                // Region spin doctor
                var spinDoctorRegionVal = ListToSpinDoctorValue(p.MetaRegion);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_region), spinDoctorRegionVal);


                // Role spin doctor
                var spinDoctorRoleVal = ListToSpinDoctorValue(p.MetaRole);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_role), spinDoctorRoleVal);


                // Topic spin doctor
                var spinDoctorTopicVal = ListToSpinDoctorValue(p.MetaTopic);
                content.SetValue(ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_spindoctor_topic), spinDoctorTopicVal);


                // Save
                Services.ContentService.Save(content);


                // Return
                return new SimpleObjectModel<bool>(true);
            }
            catch (Exception e)
            {
                var error = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message + e.StackTrace);
                throw new HttpResponseException(error);
            }
        }
        public class UpdateParam : ContentRetModel
        {
            [RequiredNumberGreaterThanZero]
            public new int? Id { get; set; }

            [Required]
            public new string Title { get; set; }
        }




        /// <summary>
        /// Build raw query for search
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        protected string BuildRawQueryForTerms(string terms)
        {
            var fieldsToSearch = new string[]
            {
                ObjectUtil.GetPropertyName<UmbracoDoctypes.Content>(x => x.content_autoExtractedContent)
            };
            
            var termArray = terms.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (termArray.Length == 0)
                return "";

            var sb = new StringBuilder();
            foreach (var field in fieldsToSearch)
            {
                var sb2 = new StringBuilder();
                foreach (var term in termArray)
                {
                    sb2.AppendFormat("+{0}:{1} ", field, term);
                }
                sb.AppendFormat("({0}) ", sb2.ToString());
            }

            var retValue = "+({0}) ".Fmt(sb.ToString());

            return retValue;
        }

        /// <summary>
        /// Serialize spin doctor from a list of Spin Doctor class.
        /// Ex: SpindoctorAttributeComponents|attributeId:Score;
        /// </summary>
        /// <param name="metas"></param>
        /// <returns></returns>
        protected string ListToSpinDoctorValue(List<ContentSpinDoctor> metas)
        {
            var list = new List<string>();
            foreach (var item in metas) list.Add(item.ToString());

            var ret = "{0}|{1}".Fmt(
                ObjectUtil.GetPropertyName<Constant.SpinDoctor>(x => x.SpindoctorAttributeComponents),
                list.JoinString(",")
                );

            return ret;
        }

    }


}
