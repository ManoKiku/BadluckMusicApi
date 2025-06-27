using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSizeAttribute(int maxSizeInMB)
        {
            _maxSize = maxSizeInMB * 1024 * 1024;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return new ValidationResult($"Invalid data type. Required 'IFormFile'.");
            
            if (file.Length > _maxSize)
                return new ValidationResult($"Max file size is: {_maxSize / (1024 * 1024)} MB");
            
            return ValidationResult.Success;
        }
    }
}
