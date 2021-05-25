using System;
namespace sppenyakitlambung.Utilities.Models.Validation
{
    public class ValidationRule
    {
        public ValidationRule() { }

        public string PropertyPath { get; set; }
        public string ValidationMessage { get; set; }

        public virtual bool Check(object value)
        {
            return value != null;
        }
    }
}
