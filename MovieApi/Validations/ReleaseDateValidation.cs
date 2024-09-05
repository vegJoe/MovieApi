using System.ComponentModel.DataAnnotations;

namespace MovieApi.Validations
{
    public class DateLimitAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            // Kontrollera att värdet inte är null och är ett heltal
            if (value != null && int.TryParse(value.ToString(), out int releaseDate))
            {
                // Konvertera releaseDate till ett datumformat
                var releaseDateString = releaseDate.ToString();
                if (DateTime.TryParseExact(releaseDateString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Kolla om releaseDate är större än dagens datum
                    if (parsedDate > DateTime.Now)
                    {
                        return new ValidationResult("ReleaseDate cannot be in the future.");
                    }
                }
                else
                {
                    return new ValidationResult("Invalid date format. Use yyyyMMdd format.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
