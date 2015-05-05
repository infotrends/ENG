using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NPOI.SS.Formula.Functions;

namespace InfoTrendsAPI
{
    public class Constants
    {
        /// <summary>
        /// InfoTrends
        /// </summary>
        public class InfoTrends
        {
            public const string CHANNEL_ID = "383CE96A-9AEC-43B9-8499-A6230AAEB1FD";

            public const string COOKIE_DOMAIN = ".infotrends.com";
           
        }


        /// <summary>
        /// Dropbox App
        /// </summary>
        public class DropboxApp
        {
            // Paths of dropbox
            public class Path
            {
                public const string PUBLIC_VIDEOS_GUIDES = "Public/Videos/Guides/";
            }


            /// <summary>
            /// This app is created using EU_DROPBOX account.
            /// Since all InfoTrends employees can search this Dropbox so
            /// access token & secret already authenticated with EU_DROPBOX account.
            /// </summary>
            public class CloudSearch
            {
                public const string APP_KEY = "8zf0csfswm8yz0w";
                public const string APP_SECRET = "w50cq0pbw5l3obt";
                public const string ACCESS_TOKEN = "wrpyv76bgkv8sz3";
                public const string ACCESS_SECRET = "rb2q01q27932s03";
            }


            /// <summary>
            /// This app is created using WEBMASTER account.
            /// Since all InfoTrends employees can search this Dropbox so
            /// access token & secret already authenticated with WEBMASTER account.
            /// </summary>
            public class Webmaster
            {
                public const string APP_KEY = "afnu3nnhgos81hp";
                public const string APP_SECRET = "8whq71yn5emnmg5";
                public const string ACCESS_TOKEN = "ku7jtzcch71gbc5";
                public const string ACCESS_SECRET = "b5pcczi68myvala";
            }

        }


        /// <summary>
        /// Survey Gizmo App
        /// </summary>
        public class SurveyGizmoApp
        {
            public class Metrics
            {
                public const string CONSUMER_KEY = "d651f5660adfa1bbc51970f054bfe94b04f54fac1";
                public const string CONSUMER_SECRET = "8f713801ee5cddb79b805b318e56e269";
                public const string ACCESS_TOKEN = "f746d7a9d97cc86f8668bf17cb1ec03304f55038d";
                public const string ACCESS_SECRET = "3a565c84cd254c641381b30d9a25d45c";

                public const string USERNAME = "cjones@infotrends.com";
                public const string PASSWORD = "infotrends12";
            }
        }



        /// <summary>
        /// Riak
        /// </summary>
        public class Riak
        {
            public const string RIAK_URL
                = "http://192.168.100.47:8098/riak";

            public const string RIAK_MAPRED_URL
                = "http://192.168.100.47:8098/mapred";
        }



        /// <summary>
        /// Cookie Name
        /// </summary>
        public class CookieName
        {
            public const string CHANNEL_ID
                = "CHANNEL";
            
            public const string SESSION_NUM
                = "SESSIONNUM";

            public const string USERNAME
                = "ENCUSERNAME";

            public const string PASSWORD
                = "ENCPASSWORD";

            public const string UG_CURRENT_MESSAGE_ID
                = "UGCURRENTMESSAGEID";

            /// <summary>
            /// Legacy Cookie names used by www.infotrends.com because it is integrated into the system of client.
            /// For the future, these will be removed if it is approved by Chris Jones
            /// </summary>
            public const string LEGACY_USERNAME_COOKIE_NAME
                = "cappassword";

            public const string LEGACY_PASSWORD_COOKIE_NAME
                = "capuser";
        }


        /// <summary>
        /// Encryption Key
        /// </summary>
        public class EncryptionKey
        {
            public const string ENC_CC_KEY
                = "E8E4C3A6-BF57-6485-AC9B-F958CA27CAF8";
        }


        /// <summary>
        /// Common Url
        /// </summary>
        public class CommonUrl
        {
            public const string COMMON_IMAGE
                = "http://webservices.infotrends.com/common/images/";

            public const string WEBSERVICES
                = "http://webservices.infotrends.com/v2.3/rest.aspx";

            public const string ATTACHMENT_DOWNLOAD
                = "http://media.infotrends.com/functions/attachmentLib.ashx";

            public const string HTML_TO_PDF_URL 
                = "http://webservices.infotrends.com/pdf/htmltopdf.aspx";
            
        }


        /// <summary>
        /// Recaptcha
        /// </summary>
        public class Recaptcha
        {
            public const string PUBLIC_KEY = "6LeLt84SAAAAANkk3J-wTn3sOz0euPhovpFouFB6";
            public const string PRIVATE_KEY = "6LeLt84SAAAAADgKGJRjms4nkVT0CyIVRNbIJsEG";
        }


        /// <summary>
        /// session life time in hour MUST BE THE SAME AS eprise
        /// </summary>
        public const int SESSION_EXPIRE_HOUR 
            = 4;


        /// <summary>
        /// valid request life in min
        /// </summary>
        public const int MAX_LIFE_OF_REQUEST 
            = 5;

        

        /// <summary>
        /// timestamp start
        /// </summary>
        public static DateTime TIMESTAMP_START 
            = new DateTime(1970, 1, 1, 0, 0, 0, 0);


        /// <summary>
        /// Exchange Server (SMTP)
        /// </summary>
        public const string HOST_EXCHANGE_SERVER 
            = "192.168.100.9";


        public const string ESERVICES_EMAIL
            = "eservices@infotrends.com";


    }
}
