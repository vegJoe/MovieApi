using MovieApi.Data;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Validations
{
    public class TitleIsUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {

            var dbContext = (MovieApiContext)validationContext.GetService(typeof(MovieApiContext));

            var existingMovie = dbContext.Movie.FirstOrDefault(m => m.Title == value.ToString());

            if (existingMovie != null)
            {
                return new ValidationResult("The movie title must be unique.");
            }

            return ValidationResult.Success;
        }
    }
}
