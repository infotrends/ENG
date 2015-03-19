using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using SitebracoApi.Models.Auth;
using SitebracoApi.Types;
using Umbraco.Core;


namespace SitebracoApi.Services.Auth
{
    public class AuthenticateService : BaseService
    {
        private static AuthenticateService _Service;
        public static AuthenticateService Current
        {
            get
            {
                _Service = _Service != null ? _Service : new AuthenticateService();
                return _Service;
            }
        }
        


        /// <summary>
        /// Get session of current authenticated user
        /// </summary>
        /// <returns></returns>
        public MemberProfileModel GetMemberProfileFromCurrentSession()
        {
            var currentUser = Membership.GetUser();
            if (currentUser == null) return null;

            var username = currentUser.UserName;
            return GetMemberProfileByUsername(username);
        }


        /// <summary>
        /// Get member profile from email
        /// </summary>
        /// <returns></returns>
        public MemberProfileModel GetMemberProfileByUsername(string username)
        {
            var member = ApplicationContext.Current.Services.MemberService.GetByUsername(username);


            // Base case
            if (member == null)
            {
                return null;
            }


            // Get permisions
            var products = new List<BaseProductModel>();


            // Get UG
            var ugRoleCache = member.GetRoleCache<UgProductModel>(ObjectUtil.GetPropertyName<UmbracoDoctypes.Member>(x => x.member_ugRolecacheJson));
            if (ugRoleCache != null) products.Add(ugRoleCache);


            // Get SMS
            var smsRoleCache = member.GetRoleCache<SmsProductModel>(ObjectUtil.GetPropertyName<UmbracoDoctypes.Member>(x => x.member_smsRolecacheJson));
            if (smsRoleCache != null) products.Add(smsRoleCache);


            // Construct
            var ret = new MemberProfileModel
            {
                Products = products,
                UserProfile = MemberModel.From(member)
            };


            // Return
            return ret;
        }


        /// <summary>
        /// Login (Authenticate) user
        /// </summary>
        /// <param name="username">Username to login</param>
        /// <param name="password">Password to login</param>
        /// <param name="doRemember">Whether to create an encrypted cookie to save their username & password</param>
        public MemberProfileModel Login(string username, string password, bool doRemember)
        {
            // Validate
            if (username.IsNullOrWhiteSpace()) throw new Exception("Invalid username.");
            if (password.IsNullOrWhiteSpace()) throw new Exception("Invalid password.");


            // Authenticating user
            var helper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext);
            var isAuthenticated = helper.Login(username, password);


            // Validate
            if (!isAuthenticated)
            {
                throw new Exception("Invalid username and password.");
            }


            // Whether to save username & password cookie
            if (doRemember)
            {

            }


            // Return
            return GetMemberProfileByUsername(username);
        }


        /// <summary>
        /// Sign user out
        /// </summary>
        public bool Logout()
        {
            var helper = new Umbraco.Web.Security.MembershipHelper(UmbracoContext);
            helper.Logout();
            return true;
        }


    }
}
