using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SitebracoApi.Services.Meta;
using SitebracoApi.Models.Meta;
using Umbraco.Web.Mvc;

namespace SitebracoApi.Controllers.Meta
{
    [PluginController(Constant.SitebracoApiModule.Meta)]
    //[Umbraco.Web.WebApi.MemberAuthorize(AllowGroup = Constant.SitebracoBasicRole.Employee)]
    public class ContentMetaController : BaseController
    {

        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListClassAttributes()
        {
            
            return ContentMetaService.Current.ListClassAttributes();
        }

        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListClientAttributes()
        {

            return ContentMetaService.Current.ListClientAttributes();
        }

        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListFocusAttributes()
        {

            return ContentMetaService.Current.ListFocusAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListIndustryAttributes()
        {
            return ContentMetaService.Current.ListIndustryAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListMarketAttributes()
        {
            return ContentMetaService.Current.ListMarketAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListPrimaryAttributes()
        {
            return ContentMetaService.Current.ListPrimaryAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListClassPrimaryAttributes([FromUri]ListClassPrimaryAttributeParam p)
        {
            try
            {
                ValidateParams(p);

                // Return
                return ContentMetaService.Current.ListPrimaryAttributes(p.ClassId);
            }
            catch (Exception e)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(message);
            }
        }
        public class ListClassPrimaryAttributeParam
        {
            [RequiredNumberGreaterThanZeroAttribute]
            public int ClassId { get; set; }
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListRegionAttributes()
        {
            return ContentMetaService.Current.ListRegionAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListRoleAttributes()
        {
            return ContentMetaService.Current.ListRoleAttributes();
        }


        [HttpGet]
        [HttpOptions]
        public List<ContentMetaModel> ListTopicAttributes()
        {
            return ContentMetaService.Current.ListTopicAttributes();
        }

    }


}
