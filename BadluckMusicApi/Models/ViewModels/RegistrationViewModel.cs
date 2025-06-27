using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [RegularExpression("^[A-Za-z0-9]{4,16}$", ErrorMessage = "Username must be between 4 and 16 characters long and contain only letters (A-Z, a-z) and numbers (0-9)")]
        [Length(4, 16)]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public required string PasswordConfirm { get; set; }
    }
}
