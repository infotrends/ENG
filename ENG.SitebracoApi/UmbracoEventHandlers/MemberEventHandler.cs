using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace SitebracoApi.UmbracoEventHandlers
{
    public class MemberEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// On Application start
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Initialize base (Important else logic of Umbraco will be broken)
            base.ApplicationStarted(umbracoApplication, applicationContext);

            // Wire event on Member saving
            MemberService.Saving += MemberService_Saving;
        }

        /// <summary>
        /// On member saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MemberService_Saving(IMemberService sender, Umbraco.Core.Events.SaveEventArgs<IMember> e)
        {
            foreach (var m in e.SavedEntities)
            {
                AddSemiColonToRoleCacheJsonField(m);
            }
        }


        /// <summary>
        /// Add a semi-colon to the end of json string
        /// </summary>
        /// <param name="member"></param>
        private void AddSemiColonToRoleCacheJsonField(IMember member)
        {   /*
             * Umbraco (front-end) will not display the json string (it displays [object Object])
             *  because Umbraco parse the string as json then put into the field.
             *  Solving adding a semi-colon (;) in front of the json the it invalid json
             *  BUT REMEMBER to remove it before reparse in our system
             */
            foreach (var p in member.Properties)
            {
                var isRolecacheJsonField = p.Alias.IndexOf("RolecacheJson", StringComparison.CurrentCultureIgnoreCase) > 1;
                
                if (!isRolecacheJsonField) 
                    continue;

                if (p.Value == null || p.Value.ToString().IsNullOrWhiteSpace()) 
                    continue;

                var v = p.Value.ToString()
                    .Trim()
                    .Trim(";")
                    ;
                
                p.Value = v.Length > 0 ? v + ";" : "";
            }
        }

    }
}
