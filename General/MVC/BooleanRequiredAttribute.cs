
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Web.Mvc;

namespace OP.General
{
    public class BooleanRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value == true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
           
            yield return new ModelClientValidationRule()
            {
                ValidationType = "booleanrequired",
                ErrorMessage = this.ErrorMessageString
            };
        }
    }
}
