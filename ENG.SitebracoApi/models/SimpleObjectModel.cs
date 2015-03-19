using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitebracoApi.Models
{
    public class SimpleObjectModel<T>
    {
        public T Data { get; set; }

        public SimpleObjectModel()
        {
        }

        public SimpleObjectModel(T data)
        {
            this.Data = data;
        }
    }
}
