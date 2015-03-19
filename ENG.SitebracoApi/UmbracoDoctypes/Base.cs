using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SitebracoApi.UmbracoDoctypes
{
    public class Base
    {
        [JsonIgnore]
        public object _SourceData { get; set; }

        /// <summary>
        /// Should be readonly
        /// </summary>
        public int? Id { get; set; }


        /// <summary>
        /// Should be readonly
        /// </summary>
        public string NodeName { get; set; }


        /// <summary>
        /// Should be readonly
        /// </summary>
        public DateTime? CreateDate { get; set; }
        

        /// <summary>
        /// Convert this object into Umbraco Model T 
        /// </summary>
        /// <typeparam name="T">Return type of umbraco model (eg. Content, Node, Member)</typeparam>
        /// <param name="commit">Whether to commit any new information.</param>
        /// <returns></returns>
        public T ToUmbracoModel<T>(bool commit = true)
        {
            if (_SourceData == null)
                return default(T);

            var ret = (T)_SourceData;

            if (!commit)
                return ret;

            return Extlib.ConvertToUmbracoModel<T>(this, ret);
        }


    }
}
