using SitebracoApi.DbEntities;
using SitebracoApi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        #region Actions

        public MemberProfileModel Login(string username, string password)
        {
            if (username.ToLower().Equals("employee@infotrends.com") && password.ToLower().Equals("fpt123@"))
            {
                var user = users.ElementAt(0);
                AddToSession(user.SessionKey, "Logged In");
                return user;
            }

            if (username.ToLower().Equals("member@infotrends.com") && password.ToLower().Equals("fpt123@"))
            {
                var user = users.ElementAt(1);
                AddToSession(user.SessionKey, "Logged In");
                return user;
            }

            if (username.ToLower().Equals("superuser@infotrends.com") && password.ToLower().Equals("fpt123@"))
            {
                var user = users.ElementAt(2);
                AddToSession(user.SessionKey, "Logged In");
                return user;
            }

            return null;
        }

        public MemberProfileModel LoginUsingSessionKey(string sessionKey)
        {
            if (sessionKey.Equals("0a18aaf8-eb79-4823-98e8-08b8b56a13de"))
            {
                var user = users.ElementAt(0);
                var currentAction = GetCurrentAction(sessionKey);

                if (currentAction != "Error")
                {
                    user.CurrentAction = currentAction;
                }

                return user;
            }

            if (sessionKey.Equals("06a999a6-30z5-495d-a246-29d29af21169"))
            {
                var user = users.ElementAt(1);
                var currentAction = GetCurrentAction(sessionKey);

                if (currentAction != "Error")
                {
                    user.CurrentAction = currentAction;
                }
                return user;
            }


            if (sessionKey.Equals("71505F43-D870-E011-878E-000C29299294"))
            {
                var user = users.ElementAt(2);
                var currentAction = GetCurrentAction(sessionKey);

                if (currentAction != "Error")
                {
                    user.CurrentAction = currentAction;
                }
                return user;
            }

            return null;
        }

        public void AddToSession(string sessionKey, string CurrentAction)
        {
            var IPAddress = HttpContext.Current.Request.UserHostAddress;

            using (var db = new SitebracoEntities())
            {
                var model = new ENG_Session();

                model.CurrentAction = CurrentAction;
                model.IPAddress = IPAddress;
                model.SessionKey = sessionKey;
                model.CreateOn = DateTime.UtcNow;

                db.ENG_Session.Add(model);

                db.SaveChanges();
            }
        }

        public string GetCurrentAction(string sessionKey)
        {
            var action = "";
            using (var db = new SitebracoEntities())
            {
                var tmp = db.ENG_Session.Where(x => x.SessionKey.Equals(sessionKey)).OrderByDescending(x => x.CreateOn).FirstOrDefault();
                if (tmp != null)
                {
                    action = tmp.CurrentAction;
                }
            }
            return action;
        }
    }
        #endregion
}
