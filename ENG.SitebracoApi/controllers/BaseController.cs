using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Umbraco.Web.WebApi;

namespace SitebracoApi.Controllers
{
    public abstract class BaseController : UmbracoApiController
    {
        /// <summary>
        /// The current web context of request
        /// </summary>
        public SitebracoWebContext WebContext
        {
            get
            {
                _WebContext = _WebContext != null ? _WebContext : SitebracoWebContext.FromCurrent();
                return _WebContext;
            }
        }
        SitebracoWebContext _WebContext;


        /// <summary>
        /// Validate param
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        protected virtual void ValidateParams(object param)
        {
            try
            {
                AttributeValidateUtil.Validate(param);
            }
            catch (Exception e)
            {
                var msg = e.InnerException != null ? e.InnerException.Message : e.Message;
                throw new Exception(msg);
            }
        }


        /// <summary>
        /// Validate whether access token is matched
        /// </summary>
        /// <param name="accessToken">Access token to compare with.</param>
        protected virtual void ValidateContextAccessToken(string accessToken)
        {
            try
            {
                var token = WebContext.Request.Headers["Authorization"];

                if (token.IsNullOrWhiteSpace()) 
                    throw new Exception();

                if (!token.Trim().Equals(accessToken.Trim(), StringComparison.CurrentCultureIgnoreCase)) 
                    throw new Exception();
            }
            catch
            {
                throw new Exception("Access denied. Invalid @Token.");
            }
        }

    }
}
