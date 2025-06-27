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
            if (value is not IFormFile file)
                return new ValidationResult("Неверный тип данных. Ожидается значение IFormFile.");

            string? extension = Path.GetExtension(file.FileName);

            if (_extensions.Contains(extension))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Тип файла '{extension ?? "null"}' не находится в списке поддерживаемых расширений: {string.Join("," ,_extensions)}");
            }
        }
    }
}
