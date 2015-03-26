using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SitebracoApi.Services;
using SitebracoApi.Services.Auth;
using SitebracoApi.Models.Auth;
using SitebracoApi.Models;
using Umbraco.Web.Mvc;
using MyUtils.Validations;

namespace SitebracoApi.Controllers.Auth
{
    [PluginController(Constant.SitebracoApiModule.Auth)]
    public class ProfileController : BaseController
    {

        [HttpPost, HttpGet]
        public MemberProfileModel Login([FromBody]string s)
        {
            try
            {
                var p = WebContext.ConvertParams<LoginParam>();
                ValidateParams(p);
                p.DoRemember = p.DoRemember == true ? true : false;
                return AuthenticateService.Current.Login(p.Username, p.Password, (bool)p.DoRemember);
            }
            catch (Exception e)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(message);
            }
            
        }
        public class LoginParam
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }

            public bool? DoRemember { get; set; }

            public string SessionKey { get; set; }
        }

        [HttpPost, HttpOptions]
        public MemberProfileModel EngLogin(LoginParam p)
        {
            try
            {
                if (p != null && !string.IsNullOrWhiteSpace(p.SessionKey))
                {
                    //validate sessionKey and get username

                    //return user profile
                    var up = AuthenticateService.Current.GetMemberProfileByUsername("jennifer.jakubowicz@okidata.com");
                    up.SessionKey = p.SessionKey;
                    return up;
                }

                ValidateParams(p);
                p.DoRemember = p.DoRemember == true ? true : false;
                var userprofile= AuthenticateService.Current.Login(p.Username, p.Password, (bool)p.DoRemember);
                userprofile.SessionKey = Guid.NewGuid().ToString();
                return userprofile;
            }
            catch (Exception e)
            {
                var message = Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                throw new HttpResponseException(message);
            }
        }


        [HttpPost, HttpGet]
        public MemberProfileModel GetCurrentSession()
        {
            return AuthenticateService.Current.GetMemberProfileFromCurrentSession();
        }


        [HttpPost, HttpGet]
        public SimpleObjectModel<bool> Logout()
        {
            var result = AuthenticateService.Current.Logout();
            return new SimpleObjectModel<bool>(result);
        }


    }
}
