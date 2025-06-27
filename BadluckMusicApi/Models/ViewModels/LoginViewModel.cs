using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required bool RememberMe { get; set; }
    }
}
