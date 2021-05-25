using System;
namespace sppenyakitlambung.Utilities.Models.Validation
{
    public class ValidationError
    {
        public string PropertyPath { get; set; }

        public string ValidationMessage { get; set; }

        public ValidationError() { }

        public ValidationError(string propertyPath, string validationMessage)
        {
            PropertyPath = propertyPath;
            ValidationMessage = validationMessage;
        }
    }
}
