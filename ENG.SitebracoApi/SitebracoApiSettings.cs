using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyUtils.WebConfig;

namespace SitebracoApi
{
    public class SitebracoApiSettings : ConfigurationSection
    {
        [ConfigurationProperty("App", Options = ConfigurationPropertyOptions.IsRequired)]
        public ConfigCollection App
        {
            get
            {
                return (ConfigCollection)this["App"];
            }
        }


        [ConfigurationProperty("Cookie", Options = ConfigurationPropertyOptions.IsRequired)]
        public ConfigCollection Cookie
        {
            get
            {
                return (ConfigCollection)this["Cookie"];
            }
        }


        [ConfigurationProperty("DbConnectionStrings", Options = ConfigurationPropertyOptions.IsRequired)]
        public ConfigCollection DbConnectionStrings
        {
            get
            {
                return (ConfigCollection)this["DbConnectionStrings"];
            }
        }



        /// <summary>
        /// Get SitecoreApiSettings
        /// </summary>
        /// <returns></returns>
        public static SitebracoApiSettings GetConfig()
        {
            return (SitebracoApiSettings)ConfigurationManager.GetSection("SitebracoApiSettings");
        }


    }
}
