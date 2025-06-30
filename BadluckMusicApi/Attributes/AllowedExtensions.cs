using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Attributes
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensions(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not IFormFile file)
                return new ValidationResult("Invalid data type. Required 'IFormFile'.");

            string? extension = Path.GetExtension(file.FileName);

            if (_extensions.Contains(extension))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"File exteinsion '{extension ?? "null"}' isnt in list of available extensions: {string.Join("," ,_extensions)}");
            }
        }
    }
}
