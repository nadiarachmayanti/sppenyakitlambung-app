using System;
using sppenyakitlambung.Helper;

namespace sppenyakitlambung.Utilities.Models.Validation.Rules
{
    public class IsNotNullOrEmptyRule : ValidationRule
    {
        public IsNotNullOrEmptyRule() { }

        public override bool Check(object obj)
        {
            if (obj != null)
            {
                SerializationHelper.GetPropertyValue(obj, PropertyPath, out object propertyValue);

                if (propertyValue == null)
                {
                    return false;
                }
                else
                {
                    if (propertyValue is string text && string.IsNullOrWhiteSpace(text))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
