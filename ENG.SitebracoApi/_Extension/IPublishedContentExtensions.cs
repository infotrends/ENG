using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace SitebracoApi
{
    public static class IPublishedContentExtensions
    {
        /// <summary>
        /// Get property value from this  generic published content.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IPublishedContent value, string alias, T defaultValue)
        {
            return Extlib.GetValue<T>(value, alias, defaultValue);
        }

    }
}
