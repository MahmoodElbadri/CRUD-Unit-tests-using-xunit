using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(Object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            if (!isValid)
            {
                throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
