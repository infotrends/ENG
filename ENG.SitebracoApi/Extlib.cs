using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Examine;
using SitebracoApi.Types;
using umbraco;
using umbraco.interfaces;
using umbraco.NodeFactory;
using Umbraco.Core.Models;

namespace SitebracoApi
{
    public class Extlib
    {
        /// <summary>
        /// Get property value from this content.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(IContent value, string alias, T defaultValue)
        {
            try
            {
                var v = value.GetValue(alias).ToString();
                var t = typeof(T);
                return (T)ChangeType(t, v);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get property value from this node.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(INode value, string alias, T defaultValue)
        {
            try
            {
                var v = value.GetProperty(alias).Value;
                var t = typeof(T);
                return (T)ChangeType(t, v);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get property value from this  generic published content.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exist.</param>
        /// <returns></returns>
        public static T GetValue<T>(IPublishedContent value, string alias, T defaultValue)
        {
            try
            {
                var v = value.GetProperty(alias).Value.ToString();
                var t = typeof(T);
                return (T)ChangeType(t, v);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get property value of this member.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="alias">Property name</param>
        /// <param name="defaultValue">Default value if property is null or does not exists.</param>
        /// <returns></returns>
        public static T GetValue<T>(IMember value, string alias, T defaultValue)
        {
            try
            {
                var v = value.GetValue(alias).ToString();
                var t = typeof(T);
                return (T)ChangeType(t, v);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Convert a string to a type
        /// </summary>
        /// <param name="toType">Type to convert to</param>
        /// <param name="fromValue">Value to convert</param>
        /// <returns></returns>
        public static object ChangeType(Type toType, string fromValue)
        {
            var v = fromValue;
            var t = toType;

            if (t == typeof(Guid))
            {
                return Convert.ChangeType(Guid.Parse(v), t);
            }

            if (t.IsEnum)
            {
                return Enum.Parse(t, v, true);
            }

            return Convert.ChangeType(v, t);
        }

        /// <summary>
        /// Convert a Umbraco model (eg. Content, Node, Member) to type T
        /// </summary>
        /// <typeparam name="TOutput">Return type</typeparam>
        /// <param name="data">Input object to convert to T</param>
        /// <returns></returns>
        public static TOutput ConvertFromUmbracoModel<TOutput>(object data)
        {
            
            if (data == null)
                return default(TOutput);


            var typeOfData = data.GetType();
            if (typeOfData != typeof(INode) && typeOfData != typeof(Node) &&
                typeOfData != typeof(IContent) && typeOfData != typeof(Content) &&
                typeOfData != typeof(IMember) && typeOfData != typeof(Member))
                return default(TOutput);


            var id = 0;
            var nodeName = "";
            var createDate = (DateTime?)null;
            var dict = new Dictionary<string, object>();


            if (typeOfData == typeof(IContent) || typeOfData == typeof(Content))
            {
                var content = (IContent)data;
                id = content.Id;
                nodeName = content.Name;
                createDate = content.CreateDate;
                foreach (var p in content.Properties)
                    dict[p.Alias] = p.Value;
            }


            if (typeOfData == typeof(INode) || typeOfData == typeof(Node))
            {
                var node = (INode)data;
                id = node.Id;
                nodeName = node.Name;
                createDate = node.CreateDate;
                foreach (var p in node.PropertiesAsList)
                    dict[p.Alias] = p.Value;
            }


            if (typeOfData == typeof(IMember) || typeOfData == typeof(Member))
            {
                var member = (IMember)data;
                id = member.Id;
                nodeName = member.Name;
                createDate = member.CreateDate;
                foreach (var p in member.Properties)
                    dict[p.Alias] = p.Value;
            }


            // Convert
            var jsonStr = dict.ToJsonString(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            System.IO.File.WriteAllText("C:\\inetpub\\sitebraco\\Test.txt", jsonStr);
            var ret = jsonStr.ToObject<TOutput>(new Newtonsoft.Json.Converters.IsoDateTimeConverter());


            // Set property @_SourceObject
            var sourceDataPropName = ObjectUtil.GetPropertyName<UmbracoDoctypes.Base, object>(x => x._SourceData);
            ret.GetType().GetProperty(sourceDataPropName).SetValue(ret, data);


            // Set property @Id
            var idPropName = ObjectUtil.GetPropertyName<UmbracoDoctypes.Base>(x => x.Id);
            ret.GetType().GetProperty(idPropName).SetValue(ret, id);


            // Set property @NodeName
            var nodeNamePropName = ObjectUtil.GetPropertyName<UmbracoDoctypes.Base>(x => x.NodeName);
            ret.GetType().GetProperty(nodeNamePropName).SetValue(ret, nodeName);


            // Set property @CreateDate
            var createDatePropName = ObjectUtil.GetPropertyName<UmbracoDoctypes.Base>(x => x.CreateDate);
            ret.GetType().GetProperty(createDatePropName).SetValue(ret, createDate);


            // Return
            return ret;
        }

        /// <summary>
        /// Convert an object to Umbraco model (eg. Content, Node, Member)
        /// </summary>
        /// <typeparam name="TOutput">Return type</typeparam>
        /// <param name="data">Input object to convert to Umbraco T Model</param>
        /// <param name="holder">The data will be set into this holder</param>
        /// <returns></returns>
        public static TOutput ConvertToUmbracoModel<TOutput>(object data, TOutput holder)
        {

            if (data == null)
                return holder;


            var typeOfHolder = holder.GetType();
            if (typeOfHolder != typeof(INode) && typeOfHolder != typeof(Node) &&
                typeOfHolder != typeof(IContent) && typeOfHolder != typeof(Content) &&
                typeOfHolder != typeof(IMember) && typeOfHolder != typeof(Member))
                return holder;


            var dict = new Dictionary<string, object>();
            var typeOfData = data.GetType();
            foreach (var p in typeOfData.GetProperties())
            {
                var name = p.Name.ToLower();
                var value = p.GetValue(data);
                dict[name] = value;
            }

            
            if (typeOfHolder == typeof(IContent) || typeOfHolder == typeof(Content))
            {
                var content = (IContent)holder;
                foreach (var p in content.Properties)
                {
                    object o;
                    if (dict.TryGetValue(p.Alias.ToLower(), out o))
                        p.Value = o;
                }
            }


            if (typeOfHolder == typeof(INode) || typeOfHolder == typeof(Node))
            {
                var node = (INode)holder;
                foreach (var p in node.PropertiesAsList)
                {
                    object o;
                    if (dict.TryGetValue(p.Alias.ToLower(), out o))
                        node.SetProperty(p.Alias, o);
                }
            }


            if (typeOfHolder == typeof(IMember) || typeOfHolder == typeof(Member))
            {
                var member = (IMember)holder;
                foreach (var p in member.Properties)
                {
                    object o;
                    if (dict.TryGetValue(p.Alias.ToLower(), out o))
                        p.Value = o;
                }
            }


            // Return
            return holder;
        }

        
        /// <summary>
        /// Recursively find all sub nodes from a node.
        /// </summary>
        /// <param name="node">Node to start finding its sub nodes.</param>
        /// <param name="depth">The depth to recursively find. If set to NULL, recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only nodes that are allowed to see.</param>
        /// <param name="filterDoctypes">Include only provided filter doctypes</param>
        /// <returns></returns>
        public static List<INode> GetDescendantOrSelfNodes(INode node, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            // Return value
            var retList = new List<INode>();


            // Base case 1
            if (node == null)
            {
                return retList;
            }


            // Base case 2
            if (depth == 0)
            {
                return retList;
            }


            // Whether to return this node
            bool isValid = true;


            // Ensure has access to this node
            if (isValid == true && includeHasAccessOnly == true)
            {
                int nodeId = node.Id;
                string nodePath = node.Path;
                isValid = library.HasAccess(nodeId, nodePath);
            }


            // Ensure to return the correct node as specified in filterDoctypes
            if (isValid == true && filterDoctypes.Length > 0)
            {
                string nodeAlias = node.NodeTypeAlias;
                var docTypes = new List<string>(filterDoctypes);
                isValid = docTypes.Exists(x => x.Trim().Equals(nodeAlias, StringComparison.CurrentCultureIgnoreCase));
            }


            // Whether to return this node
            if (isValid)
            {
                retList.Add(node);
            }


            // Whether to go through the children node
            var children = node.ChildrenAsList;
            if (children.Count > 0)
            {
                uint? d = depth == null ? null : depth - 1;
                foreach (INode iNode in children)
                {
                    var sub = GetDescendantOrSelfNodes(iNode, d, includeHasAccessOnly, filterDoctypes);
                    retList.AddRange(sub);
                }
            }


            // return
            return retList;
        }

        /// <summary>
        /// Recursively find all sub nodes from a node.
        /// </summary>
        /// <param name="node">Node to start finding its sub nodes.</param>
        /// <param name="depth">The depth to recursively find. If set to NULL, recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only nodes that are allowed to see.</param>
        /// <param name="filterDoctypes">Include only provided filter doctypes</param>
        /// <returns></returns>
        public static List<Node> GetDescendantOrSelfNodes(Node node, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            var iNode = (INode)node;
            var results = GetDescendantOrSelfNodes(iNode, depth, includeHasAccessOnly, filterDoctypes);
            var ret = new List<Node>();

            foreach (var item in results)
                ret.Add((Node)item);

            return ret;
        }

        /// <summary>
        /// Recursively find all sub contents from a content.
        /// </summary>
        /// <param name="content">Content to start finding its sub contents.</param>
        /// <param name="depth">The depth to recursively find. If set to NULL, recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only nodes that are allowed to see.</param>
        /// <param name="filterDoctypes">Include only provided filter doctypes</param>
        /// <returns></returns>
        public static List<IContent> GetDescendantOrSelfContents(IContent content, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            // Return value
            var retList = new List<IContent>();


            // Base case 1
            if (content == null)
            {
                return retList;
            }


            // Base case 2
            if (depth == 0)
            {
                return retList;
            }


            // Whether to return this node
            bool isValid = true;


            // Ensure has access to this node
            if (isValid == true && includeHasAccessOnly == true)
            {
                int contentId = content.Id;
                string contentPath = content.Path;
                isValid = library.HasAccess(contentId, contentPath);
            }


            // Ensure to return the correct node as specified in filterDoctypes
            if (isValid == true && filterDoctypes.Length > 0)
            {
                string nodeAlias = content.ContentType.Alias;
                var docTypes = new List<string>(filterDoctypes);
                isValid = docTypes.Exists(x => x.Trim().Equals(nodeAlias, StringComparison.CurrentCultureIgnoreCase));
            }


            // Whether to return this node
            if (isValid)
            {
                retList.Add(content);
            }


            // Whether to go through the children node
            var children = content.Children();
            if (children.Count() > 0)
            {
                uint? d = depth == null ? null : depth - 1;
                foreach (IContent iContent in children)
                {
                    var sub = GetDescendantOrSelfContents(iContent, d, includeHasAccessOnly, filterDoctypes);
                    retList.AddRange(sub);
                }
            }


            // return
            return retList;
        }

        /// <summary>
        /// Recursively find all sub contents from a content.
        /// </summary>
        /// <param name="content">Content to start finding its sub contents.</param>
        /// <param name="depth">The depth to recursively find. If set to NULL, recursively find all depth.</param>
        /// <param name="includeHasAccessOnly">Whether to filter only nodes that are allowed to see.</param>
        /// <param name="filterDoctypes">Include only provided filter doctypes</param>
        /// <returns></returns>
        public static List<Content> GetDescendantOrSelfContents(Content content, uint? depth, bool includeHasAccessOnly, params string[] filterDoctypes)
        {
            var iContent = (IContent)content;
            var results = GetDescendantOrSelfContents(iContent, depth, includeHasAccessOnly, filterDoctypes);
            var ret = new List<Content>();

            foreach (var item in results)
                ret.Add((Content)item);

            return ret;
        }


        /// <summary>
        /// Search examine index
        /// </summary>
        public static List<SearchResult> SearchExamine(InfoTrendsExamineSearcherType examineSearcher, string luceneQueryString, int skip, int take, out int totalResults)
        {
            return SearchExamine(examineSearcher.ToString(), luceneQueryString, skip, take, out totalResults);
        }
        public static List<SearchResult> SearchExamine(string examineSearcher, string luceneQueryString, int skip, int take, out int totalResults)
        {
            var rawResults = SearchExamine(examineSearcher, luceneQueryString)
                .AsEnumerable();

            totalResults = rawResults.Count();

            var ret = rawResults
                .OrderByDescending(x => x.Score)
                .Skip(skip)
                .Take(take)
                .ToList<SearchResult>()
                ;


            return ret;
            
        }
        public static ISearchResults SearchExamine(InfoTrendsExamineSearcherType examineSearcher, string luceneQueryString)
        {
            var searcher = examineSearcher.ToString();
            return SearchExamine(searcher, luceneQueryString);
        }
        public static ISearchResults SearchExamine(string examineSearcher, string luceneQueryString)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection[examineSearcher];
            var criteria = searcher.CreateSearchCriteria();
            var query = criteria.RawQuery(luceneQueryString);
            return searcher.Search(query);
        }




    }
}
