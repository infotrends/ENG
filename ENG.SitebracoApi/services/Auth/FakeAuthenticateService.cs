using SitebracoApi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Services.Auth
{
    public class FakeAuthenticateService
    {
        #region FakedUsers
        public static List<MemberProfileModel> users = new List<MemberProfileModel>
        {
            new MemberProfileModel {
                UserProfile = new MemberModel {
                    Email = "employee@infotrends.com",
                    First = "Imma",
                    Last = "Empoyee",
                    ContactId = new Guid("53505F43-D870-E011-878E-000C29299294"),
                    SuperUser = false,
                    MemberType = Types.UmbracoMemberType.Employee
                },
                SessionKey = "0a18aaf8-eb79-4823-98e8-08b8b56a13de"
            },
            new MemberProfileModel {
                UserProfile = new MemberModel {
                    Email = "member@infotrends.com",
                    First = "Imma",
                    Last = "Member",
                    ContactId = new Guid("9106183a-6bf3-4564-8a85-f1f65ddca2fb"),
                    SuperUser = false,
                    MemberType = Types.UmbracoMemberType.Member,                    
                },
                SessionKey = "06a999a6-30z5-495d-a246-29d29af21169"
            },
            new MemberProfileModel {
                UserProfile = new MemberModel {
                    Email = "superuser@infotrends.com",
                    First = "Imma",
                    Last = "SuperUser",
                    ContactId = new Guid("87505F43-D870-E011-878E-000C29299294"),
                    SuperUser = true,
                    MemberType = Types.UmbracoMemberType.SuperUser
                },
                SessionKey = "71505F43-D870-E011-878E-000C29299294"
            },
        };
        #endregion

        public MemberProfileModel Login(string username, string password)
        {
            if (username.ToLower().Equals("employee@infotrends.com") && password.ToLower().Equals("fpt123@"))
                return users.ElementAt(0);

            if (username.ToLower().Equals("member@infotrends.com") && password.ToLower().Equals("fpt123@"))
                return users.ElementAt(1);

            if (username.ToLower().Equals("superuser@infotrends.com") && password.ToLower().Equals("fpt123@"))
                return users.ElementAt(2);

            return null;
        }
    }
}
