//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SitebracoApi.DbEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class cmsMacro
    {
        public cmsMacro()
        {
            this.cmsMacroProperties = new HashSet<cmsMacroProperty>();
        }
    
        public int id { get; set; }
        public bool macroUseInEditor { get; set; }
        public int macroRefreshRate { get; set; }
        public string macroAlias { get; set; }
        public string macroName { get; set; }
        public string macroScriptType { get; set; }
        public string macroScriptAssembly { get; set; }
        public string macroXSLT { get; set; }
        public bool macroCacheByPage { get; set; }
        public bool macroCachePersonalized { get; set; }
        public bool macroDontRender { get; set; }
        public string macroPython { get; set; }
    
        public virtual ICollection<cmsMacroProperty> cmsMacroProperties { get; set; }
    }
}
