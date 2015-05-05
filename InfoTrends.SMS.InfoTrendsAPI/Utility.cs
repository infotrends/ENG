using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Net.Mail;

using CommonLibrary.Utility;

using InfoTrendsCommon.DataContext;

using InfoTrendsAPI.Type;


using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;

using Microsoft.Crm.Sdk.Messages;

using Recaptcha;

namespace InfoTrendsAPI
{
    public class Utility
    {
        /// <summary>
        /// unreserve character for
        /// </summary>
        private const string UNRESERVED_CHARS 
            = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~()'";


        /// <summary>
        /// File Extensions
        /// </summary>
        public const string ALLOWED_IMAGE_EXTENSIONS = ":png:gif:jpg:jpeg:bmp:";
        public const string ALLOWED_FILE_EXTENSIONS = ":avi:bmp:doc:docx:gif:html:jpg:jpeg:mov:mp3:mpeg:ogg:pdf:png:ppt:pptx:rtf:swf:wav:xls:xlsx:";
        public const string VIDEO_FILE_EXTENSIONS = ":avi:f4v:mov:mp4:mpeg:ogg:";



        /// <summary>
        /// Replace < with %lt%
        /// Replace > with %rt%
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeOAuthHtmlString(string value)
        {
            value = value.Replace("<", "%lt%");
            value = value.Replace(">", "%rt%");
            return value;
        }


        /// <summary>
        /// Replace %lt% with <
        /// Replace %rt% with >
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecodeOAuthHtmlString(string value)
        {
            value = value.Replace("%lt%", "<");
            value = value.Replace("%rt%", ">");
            value = value.Replace("<script", "");
            value = value.Replace("</script", "");
            return value;
        }


        /// <summary>
        /// deep cloning of an object
        /// </summary>
        public static T Clone<T>(T source)
        {
            //source must be serializable
            if (!typeof(T).IsSerializable) throw new ArgumentException("The type must be serializable.", "source");

            //don't serialize a null object, simply return default for that object
            if (Object.ReferenceEquals(source, null)) return default(T);

            //clone
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// convert the @p into role name
        /// example: 
        /// CutRolDB        -> cutroldb
        /// Cut-Sheet       -> cutsheetdb
        /// Variable Data   -> variabledb
        /// </summary>
        public static string ConvertToRoleName(string p)
        {
            p = p.ToLower().Trim();
            p = p.Replace(" ", "");
            p = p.Replace("-", "");
            string dbStr = p.Substring(p.Length - 2);
            if (dbStr == "db") return p;
            return (p + "db");
        }
        


        /// <summary>
        /// convert to simple date time => eg: 20110502010302
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Int64 ToSimpleTime(DateTime date)
        {
            string dateStr = date.ToString("yyyyMMddHHmmss");
            return Convert.ToInt64(dateStr);
        }



        /// <summary>
        /// making attachment download link
        /// </summary>
        public static string GetAttachmentDownloadLink(string attachmentId)
        {
            return Constants.CommonUrl.ATTACHMENT_DOWNLOAD + "?action=download&id=" + attachmentId;
        }




        /// <summary>
        /// get the file icon url that the extension stands for
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileIconUrl(string filename)
        {
            // get extension
            string ext = Path.GetExtension(filename);


            // list of extension
            List<string[]> list = new List<string[]>();
            list.Add(new string[] { "avi","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "bmp","psd.png" });
            list.Add(new string[] { "doc","docrtf.png" });
            list.Add(new string[] { "docx","docrtf.png" });
            list.Add(new string[] { "gif","psd.png" });
            list.Add(new string[] { "htm", "htmlhtmFF.png" });
            list.Add(new string[] { "html","htmlhtmFF.png" });
            list.Add(new string[] { "jpg","psd.png" });
            list.Add(new string[] { "jpeg","psd.png" });
            list.Add(new string[] { "mov","mov.png" });
            list.Add(new string[] { "mp3","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "mpeg","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "ogg","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "pdf","pdf.png" });
            list.Add(new string[] { "png","psd.png" });
            list.Add(new string[] { "ppt","ppt.png" });
            list.Add(new string[] { "pptx","ppt.png" });
            list.Add(new string[] { "rtf","docrtf.png" });
            list.Add(new string[] { "swf","swf.png" });
            list.Add(new string[] { "wav","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "wmv","mpegaviwavoggmp3.png" });
            list.Add(new string[] { "xls","xls.png" });
            list.Add(new string[] { "xlsx","xls.png" });
            list.Add(new string[] { "rar","ziprar.png" });
            list.Add(new string[] { "zip","ziprar.png" });

            // find
            string retStr = "generic.png";
            foreach (string[] iExt in list)
            {
                if (("." + iExt[0]) == ext)
                {
                    retStr = iExt[1];
                    break;
                }
            }

            // final
            string finalExt = Constants.CommonUrl.COMMON_IMAGE.TrimEnd('/') + "/fileIcons/" + retStr;

            // return
            return finalExt;
        }


        /// <summary>
        /// Validate file extension
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="useAllowExtension"></param>
        /// <returns></returns>
        public static bool IsValidExtension(string filename, string extensionSet)
        {
            string ext = ":" + Path.GetExtension(filename).ToLower() + ":";
            ext = ext.Replace(".", "");
            if (extensionSet.IndexOf(ext) > -1) return true;
            return false;
        }


        /// <summary>
        /// Log
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Log(string path, string content)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine(content);
                w.Flush();
                w.Close();
            }
        }


        /// <summary>
        /// Make Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <returns></returns>
        public static HttpCookie MakeCookie(string cookieName, object cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue.ToString());
            cookie.Domain = Constants.InfoTrends.COOKIE_DOMAIN;
            cookie.HttpOnly = false;
            return cookie;
        }

    }

}
