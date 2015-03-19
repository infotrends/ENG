using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SitebracoApi
{
    public class SitebracoWebContext : WebApiContext
    {
        


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SitebracoWebContext(HttpContext context)
            : base(context)
        {
            
        }


        /// <summary>
        /// Get Header Value
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        private string GetHeaderValue(string headerName)
        {
            try
            {
                string headerKey = Request.Headers.AllKeys.Single(x => x.Equals(headerName));
                return Request.Headers[headerKey];
            }
            catch
            {
                return "";
            }

        }




        #region Static Utility

        /// <summary>
        /// Base web context
        /// </summary>
        /// <returns></returns>
        public static SitebracoWebContext FromCurrent()
        {
            return From(System.Web.HttpContext.Current);
        }


        /// <summary>
        /// Custom web context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SitebracoWebContext From(HttpContext context)
        {
            return new SitebracoWebContext(context);
        }


        #endregion


    }
}
