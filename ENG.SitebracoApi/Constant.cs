using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi
{
    public class Constant
    {
        /// <summary>
        /// Max length of the content node name
        /// </summary>
        public const int ContentNodeNameMaxLength = 30;


        /// <summary>
        /// Umbraco Database Connection
        /// </summary>
        private static string _UmbracoConnectionString;
        public static string UmbracoConnectionString
        {
            get
            {
                if (_UmbracoConnectionString == null)
                {
                    var connectionString = string.Empty;

                    const string umbracoDsn = "umbracoDbDSN";

                    var databaseSettings = ConfigurationManager.ConnectionStrings[umbracoDsn];
                    if (databaseSettings != null)
                    {
                        connectionString = databaseSettings.ConnectionString;
                    }

                    // Set
                    _UmbracoConnectionString = connectionString;
                }
                return _UmbracoConnectionString;
            }
        }


        public class SitebracoApiModule
        {
            public const string Auth = "Auth";
            public const string Content = "Content";
            public const string ContentEditor = "ContentEditor";
            public const string InternalCrm = "InternalCrm";
            public const string Meta = "Meta";
            public const string Sms = "Sms";

            public const string Sinh = "Sinh";
        }
        public class SitebracoApiSettings
        {
            public string ContentClassificationDb = "";
            public string DynamicCRM = "";
        }
        public class SitebracoBasicRole
        {
            public const string Employee = "Employee";
            public const string EmployeeOrMember = "Employee, Member";
            public const string Member = "Member";
        }


        public class ContentType
        {
            public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            public const string Html = "text/html";
            public const string Pdf = "application/pdf";
            public const string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }

        public class RiakSolr
        {
            public class BucketType
            {
                public string Solr { get; set; }
            }
            public class ConfigSection
            {
                public string riakSolrConfig { get; set; }
            }
        }

        public class SpinDoctor
        {
            public string SpindoctorAttributeComponents { get; set; }
        }

        public class WebConfig
        {
            public class App
            {
                public string CrmAccessToken { get; set; }
                public string EncryptionKey { get; set; }

                public string ContentHandler { get; set; }
                public string ContentLinkHandler { get; set; }
                public string ContentMediaHandler { get; set; }

                public string InteropEndpoint { get; set; }

                public string UmbracoContentFolerId { get; set; }
            }
        }


        public class EngRiakConfig
        {
            public const string BUKET_TYPE = "InfoTrendsLog";

            public const string SOLR_CONFIG = "riakSolrConfig";
        }

    }
}
