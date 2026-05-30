using System;
using System.ComponentModel.DataAnnotations;

namespace GestionTareasApi.Validation
{
    public class FutureOrPresentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateTime)
            {
                if (dateTime.Date < DateTime.Today)
                {
                    return new ValidationResult("La fecha de vencimiento no puede ser menor a la fecha actual.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
