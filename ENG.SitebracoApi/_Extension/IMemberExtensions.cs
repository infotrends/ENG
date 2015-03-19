using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace SitebracoApi
{
    public static class IMemberExtensions
    {
        /// <summary>
        /// Get property value of this member.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exists.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IMember value, string alias, T defaultValue)
        {
            return Extlib.GetValue<T>(value, alias, defaultValue);
        }


        /// <summary>
        /// Get roleCache structure of this member
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="roleCachePropertyName">Property name of rolecache field to get from.</param>
        /// <returns></returns>
        public static T GetRoleCache<T>(this IMember value, string roleCachePropertyName)
        {
            try
            {
                var v = GetValue<string>(value, roleCachePropertyName, "")
                    .Trim()
                    .Trim(";")
                    ;
                
                return v.ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }

    }
}
