using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using Umbraco.Core.Models;

namespace SitebracoApi
{
    public static class IContentExtensions
    {
        /// <summary>
        /// Get property value from this node.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IContent value, string alias, T defaultValue)
        {
            return Extlib.GetValue<T>(value, alias, defaultValue);
        }

        /// <summary>
        /// Recursively find all contents from this content and all of its sub contents.
        /// </summary>
        /// <param name="depth">The depth to recursively find. Set to null to recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only contents allowed to see.</param>
        /// <param name="filterDoctypes">Filter the result with this doctypes.</param>
        /// <returns></returns>
        public static List<IContent> GetDescendantOrSelfContents(this IContent content, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            return Extlib.GetDescendantOrSelfContents(content, depth, includeHasAccessOnly, filterDoctypes);
        }

        /// <summary>
        /// Recursively find all contents from this content and all of its sub contents.
        /// </summary>
        /// <param name="depth">The depth to recursively find. Set to null to recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only contents allowed to see.</param>
        /// <param name="filterDoctypes">Filter the result with this doctypes.</param>
        /// <returns></returns>
        public static List<Content> GetDescendantOrSelfContents(this Content content, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            return Extlib.GetDescendantOrSelfContents(content, depth, includeHasAccessOnly, filterDoctypes);
        }

    }
}
