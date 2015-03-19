using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtils.Validations;

namespace SitebracoApi
{
    /// <summary>
    /// Validate whether a field has number greater than 0
    /// </summary>
    public class RequiredNumberGreaterThanZeroAttribute : RequiredAttribute, IValidateAttribute
    {
        public override void Validate(string fieldName, object fieldValue)
        {
            base.Validate(fieldName, fieldValue);

            try
            {
                var result = int.Parse(fieldValue.ToString());
                if (result <= 0) throw new Exception();
            }
            catch
            {
                throw new Exception("Invalid @{0}".Fmt(fieldName));
            }
        }
    }
}
