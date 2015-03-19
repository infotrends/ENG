using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using SitebracoApi.Types;
using SitebracoApi.UmbracoDoctypes;
using SitebracoApi.DbEntities;
using SitebracoApi.Models.Meta;
using Umbraco.Core;


namespace SitebracoApi.Services.Meta
{
    public class ContentMetaService : BaseService
    {
        const int CLASS_PROFILE_ID = 3;
        const int INDUSTRY_PROFILE_ID = 4;
        const int MARKET_PROFILE_ID = 5;
        const int PRIMARY_PROFILE_ID = 6;
        const int REGION_PROFILE_ID = 7;
        const int FOCUS_PROFILE_ID = 8;
        const int TOPIC_PROFILE_ID = 9;
        const int CLIENT_PROFILE_ID = 1004;
        const int ROLE_PROFILE_ID = 1005;


        private static ContentMetaService _Service;
        public static ContentMetaService Current
        {
            get
            {
                _Service = _Service != null ? _Service : new ContentMetaService();
                return _Service;
            }
        }


        /// <summary>
        /// Class Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListClassAttributes()
        {
            var results = ListAttributes(CLASS_PROFILE_ID);
            return results;
        }



        /// <summary>
        /// Industry Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListIndustryAttributes()
        {
            var results = ListAttributes(INDUSTRY_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// Market Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListMarketAttributes()
        {
            var results = ListAttributes(MARKET_PROFILE_ID);
            return results;
        }

        
        /// <summary>
        /// Primary Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListPrimaryAttributes()
        {
            var results = ListAttributes(PRIMARY_PROFILE_ID);
            return results;
        }
        public List<ContentMetaModel> ListPrimaryAttributes(int classId)
        {
            using (SitebracoEntities db = new SitebracoEntities())
            {
                var results = db.new_MetaClassPrimaryView
                    .Where(x => x.ClassId == classId)
                    .OrderBy(x => x.ClassName)
                    .ToList();

                var ret = new List<ContentMetaModel>();
                foreach (var item in results)
                {
                    var o = new ContentMetaModel
                    {
                        Id = item.PrimaryId,
                        Name = item.PrimaryName
                    };
                    ret.Add(o);
                }
                
                // Return
                return ret;
            }
        }


        /// <summary>
        /// Region Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListRegionAttributes()
        {
            var results = ListAttributes(REGION_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// Role Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListRoleAttributes()
        {
            var results = ListAttributes(ROLE_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// Topic Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListTopicAttributes()
        {
            var results = ListAttributes(TOPIC_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// Client Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListClientAttributes()
        {
            var results = ListAttributes(CLIENT_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// Focus Meta Attributes
        /// </summary>
        /// <returns></returns>
        public List<ContentMetaModel> ListFocusAttributes()
        {
            var results = ListAttributes(FOCUS_PROFILE_ID);
            return results;
        }


        /// <summary>
        /// List Attributes of a Profile (Class, Industry, Market, Primary, Region, Role, Topic, Focus, Client)
        /// </summary>
        /// <param name="profileId">Profile Id to list its attributes</param>
        /// <returns></returns>
        public List<ContentMetaModel> ListAttributes(int profileId)
        {
            using (SitebracoEntities db = new SitebracoEntities())
            {
                var results = db.Components
                    .Where(x => x.ProfilingAttributeId == profileId && x.IsArchived == false)
                    .OrderBy(x => x.Name)
                    .ToList();
                var ret = Transfer(results);
                return ret;
            }
        }


        /// <summary>
        /// Transfer entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<ContentMetaModel> Transfer(List<Component> entities)
        {
            var ret = new List<ContentMetaModel>();

            foreach (var item in entities)
            {
                var o = new ContentMetaModel
                {
                    Id = item.Id,
                    Name = item.Name
                };
                ret.Add(o);
            }

            return ret;
        }




    }
}
