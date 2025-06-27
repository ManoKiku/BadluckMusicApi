using BadluckMusicApi.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Attributes
{
    public class AuthorExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not int id || id <= 0)
                return new ValidationResult("Incorrect author id");

            var services = validationContext.GetRequiredService<IServiceProvider>();

            using var scope = services.CreateScope();
            var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

            var author = authorService.GetAuthorAsync(id).GetAwaiter().GetResult();

            return author != null
                ? ValidationResult.Success
                : new ValidationResult($"Auhor with ID {id} isnt exists");
        }
    }
}