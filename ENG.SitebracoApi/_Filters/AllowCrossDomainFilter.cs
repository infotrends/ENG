using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SitebracoApi
{
    /// <summary>
    /// Allow cross domain call
    /// </summary>
    public class AllowCrossDomainFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                actionExecutedContext.Response.Content.Headers.Add("Access-Control-Allow-Origin", "*");
                actionExecutedContext.Response.Content.Headers.Add("Access-Control-Allow-Credentials", "true");
                actionExecutedContext.Response.Content.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With, Content-Type, Accept");
            }
            catch { }
        }
    }
}
