using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SitebracoApi.Services;
using SitebracoApi.Models.ContentEditor;
using SitebracoApi.DbEntities;
using SitebracoApi.Types;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using MyUtils.Validations;


namespace SitebracoApi.Controllers.ContentEditor
{

    [PluginController(Constant.SitebracoApiModule.ContentEditor)]
    [Umbraco.Web.WebApi.MemberAuthorize(AllowGroup = Constant.SitebracoBasicRole.Employee)]
    public class ContentTemplateController : BaseController
    {

        [HttpGet]
        public List<TemplateModel> GetAllTemplates([FromUri]GetAllTemplatesParam p)
        {
            try
            {
                ValidateParams(p);
                
                using (var db = new SitebracoEntities())
                {
                    var typeStr = p.Type.ToString();
                    var results = db.new_OfficeTemplate
                        .Where(x => x.Type == typeStr && x.MarkDeleteOn == null)
                        .OrderBy(x => x.Type)
                        .OrderBy(x => x.Name)
                        .ToList();

                    // Return
                    return TemplateModel.Transfer(results);
                }

            }
            catch (Exception e)
            {
                var error = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(error);
            }

        }
        public class GetAllTemplatesParam
        {
            [Required]
            public ContentEditorTemplateType? Type { get; set; }
        }


        [HttpGet]
        public TemplateModel GetTemplate([FromUri]GetTemplateParam p)
        {
            try
            {
                ValidateParams(p);

                var id = (Guid)p.Id;
                using (var db = new SitebracoEntities())
                {
                    var result = db.new_OfficeTemplate
                        .Single(x => x.Id == id);

                    return TemplateModel.TransferSingle(result);
                }
                
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public class GetTemplateParam
        {
            [Required]
            public Guid? Id { get; set; }
        }


    }


}
