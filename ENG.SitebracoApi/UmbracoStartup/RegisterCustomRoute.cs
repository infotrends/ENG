using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using System.Web.Routing;
using Umbraco.Core;


namespace SitebracoApi.UmbracoStartup
{
    public class RegisterCustomRoute : ApplicationEventHandler
    {

        /// <summary>
        /// Application Started
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            Register();
        }


        
        void Register()
        {
            // Global configuration
            var config = GlobalConfiguration.Configuration;


            // Support JSON only
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            // Using custom web api route to include namespace
            config.Services.Replace(
                typeof(IHttpControllerSelector),
                new NamespaceHttpControllerSelector(GlobalConfiguration.Configuration)
                );


            // Add custom filter
            config.Filters.Add(new AllowCrossDomainFilter());


            //string path = "C:\\inetpub\\sitebraco\\_sinh\\Registering{0}.txt".Fmt(DateTime.Now.GetTotalMilliseconds());
            //string content = "";
            //foreach (var item in config.Routes) content += item.RouteTemplate + "\n";
            //System.IO.File.WriteAllText(path, content);


        }




    }
}
