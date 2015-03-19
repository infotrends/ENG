using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace SitebracoApi.Services
{
    public abstract class BaseService
    {
        public ApplicationContext ApplicationContext { get; protected set; }
        public ServiceContext Services { get; protected set; }
        public DatabaseContext DatabaseContext { get; protected set; }
        public UmbracoHelper Umbraco { get; protected set; }
        public UmbracoContext UmbracoContext { get; protected set; }


        /// <summary>
        /// Contructor
        /// </summary>
        public BaseService()
        {
            ApplicationContext = ApplicationContext.Current;
            Services = ApplicationContext.Current.Services;
            DatabaseContext = ApplicationContext.Current.DatabaseContext;
            Umbraco = new UmbracoHelper(UmbracoContext.Current);
            UmbracoContext = UmbracoContext.Current;
        }

    }
}
