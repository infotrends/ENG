using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models
{
    public class Response<T> where T : class
    {
        public bool? success { get; set; }

        public string message { get; set; }

        public string error { get; set; }

        public T data { get; set; }
    }
}
