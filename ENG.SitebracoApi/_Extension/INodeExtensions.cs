using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using umbraco.interfaces;
using umbraco.NodeFactory;

namespace SitebracoApi
{
    public static class INodeExtensions
    {
        /// <summary>
        /// Get property value from this node.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(this INode value, string alias, T defaultValue)
        {
            return Extlib.GetValue<T>(value, alias, defaultValue);
        }

        /// <summary>
        /// Recursively find all nodes from this node and all of its sub nodes.
        /// </summary>
        /// <param name="depth">The depth to recursively find. Set to null to recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only contents allowed to see.</param>
        /// <param name="filterDoctypes">Filter the result with this doctypes.</param>
        /// <returns></returns>
        public static List<INode> GetDescendantOrSelfNodes(this INode node, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            return Extlib.GetDescendantOrSelfNodes(node, depth, includeHasAccessOnly, filterDoctypes);
        }

        /// <summary>
        /// Recursively find all nodes from this node and all of its sub nodes.
        /// </summary>
        /// <param name="depth">The depth to recursively find. Set to null to recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only contents allowed to see.</param>
        /// <param name="filterDoctypes">Filter the result with this doctypes.</param>
        /// <returns></returns>
        public static List<Node> GetDescendantOrSelfNodes(this Node node, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            return Extlib.GetDescendantOrSelfNodes(node, depth, includeHasAccessOnly, filterDoctypes);
        }

    }
}
