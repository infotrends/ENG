using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using System.Web.Security;


/// <summary>
/// For more information, look at 
/// http://www.asp.net/web-api/overview/security/basic-authentication
/// </summary>
namespace InfoTrendsAPI.HttpModules
{
    public class BasicAuthentication : IHttpModule
    {
        protected const string REALM = "My Application";


        #region IHttpModel members


        /// <summary>
        /// Start
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += new EventHandler(OnApplicationAuthenticateRequest);
            context.EndRequest += new EventHandler(OnApplicationEndRequest);
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }


        #endregion


        /// <summary>
        /// Decodes Base64 encoded string.
        /// </summary>
        /// <param name="EncodedData">The encoded data.</param>
        /// <returns></returns>
        private string Base64Decode(string EncodedData)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(EncodedData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                return new String(decoded_char);
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }


        /// <summary>
        /// Authenticates the specified credentials.
        /// </summary>
        /// <param name="Credentials">The credentials (Username and Password).</param>
        /// <param name="Roles">The string array containing roles.</param>
        /// <returns></returns>
        protected virtual GenericPrincipal Authenticate(string[] Credentials)
        {
            string[] Roles = { "test", "hello" };
            GenericPrincipal UserPrincipal = new GenericPrincipal(new GenericIdentity(Credentials[0]), Roles);
            return UserPrincipal;
        }

        

        /// <summary>
        /// Application on authenticate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
              string filename = "D:\\BasicAuth{0}.txt";
                int i = 0;
                while (System.IO.File.Exists(string.Format(filename, i)))
                {
                    i++;
                }
                System.IO.File.WriteAllText(
                    string.Format(filename, i), 
                    application.Context.Request.Headers["Authorization"]
                    );
            if (!application.Context.Request.IsAuthenticated)
            {
  

                //string sAUTH = application.Request.ServerVariables["HTTP_AUTHORIZATION"];

                //if (string.IsNullOrWhiteSpace(sAUTH))
                //{
                //    return;
                //}

                ////Received Credentials, Authenticate user
                //if (sAUTH.Substring(0, 5).ToUpper() == "BASIC")
                //{
                //    string[] sCredentials;
                //    sCredentials = Base64Decode(sAUTH.Substring(6)).Split(':');

                //    GenericPrincipal UserPrincipal = Authenticate(sCredentials);
                //    if (UserPrincipal != null)
                //    {
                //        FormsAuthentication.Authenticate(sCredentials[0], sCredentials[1]);
                //        application.Context.User = UserPrincipal;
                //    }
                //}
            }

        }


        /// <summary>
        /// If the request was unauthorized, add the WWW-Authenticate header to the response.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationEndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            if (application.Response.StatusCode == 401)
            {
                application.Response.AddHeader("WWW-Authenticate", "BASIC Realm=" + REALM);
            }
        }


    }
}
