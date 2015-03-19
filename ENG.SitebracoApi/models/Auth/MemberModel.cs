using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core;
using SitebracoApi.Types;

namespace SitebracoApi.Models.Auth
{
    public class MemberModel
    {
        [JsonIgnore]
        public IMember _UmbracoMember { get; set; }

        [JsonIgnore]
        public string Email { get; set; }


        public string First { get; set; }

        public string Last { get; set; }

        public Guid? ContactId { get; set; }

        public bool? SuperUser { get; set; }

        public string MemberTypeName
            {
            get
            {
                return MemberType.ToString();
            }
            set { }
        }
        public UmbracoMemberType MemberType { get; set; } 
        


        public static MemberModel FromEmail(string email)
        {
            if (email.IsNullOrWhiteSpace())
            {
                return null;
            }

            // Return
            var member = ApplicationContext.Current.Services.MemberService.GetByEmail(email);
            return From(member);
        }
        public static MemberModel FromUsername(string username)
        {
            if (username.IsNullOrWhiteSpace())
            {
                return null;
            }

            // Return
            var member = ApplicationContext.Current.Services.MemberService.GetByUsername(username);
            return From(member);
        }
        public static MemberModel From(IMember p)
        {
            if (p == null)
            {
                return null;
            }

            var sp = p.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var last = sp[0];
            var first = sp[1];

            Guid? contactId = p.GetValue<Guid>(
                ObjectUtil.GetPropertyName<UmbracoDoctypes.Member>(x => x.member_contactId), 
                Guid.Empty
                );

            if (contactId == Guid.Empty) 
                contactId = null;


            var isEmployee = ApplicationContext.Current.Services.MemberService
                .FindMembersInRole(UmbracoMemberType.Employee.ToString(), p.Username)
                .Count() > 0;

            
            var isSuperUser = ApplicationContext.Current.Services.MemberService
                .FindMembersInRole(UmbracoMemberType.SuperUser.ToString(), p.Username)
                .Count() > 0;
            
            
            var memberType = UmbracoMemberType.Member;
            if (isEmployee || isSuperUser) memberType = UmbracoMemberType.Employee;
            //if (isSuperUser) memberType = UmbracoMemberType.SuperUser;


            // Construct a return
            var ret = new MemberModel
            {
                _UmbracoMember = p,
                ContactId = contactId,
                Email = p.Email,
                First = first,
                Last = last,
                MemberType = memberType,
                SuperUser = isSuperUser
            };


            // Return
            return ret;
        }

    }
}
