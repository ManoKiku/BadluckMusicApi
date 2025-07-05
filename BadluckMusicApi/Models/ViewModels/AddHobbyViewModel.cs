using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class AddHobbyViewModel
    {
        [Required]
        [AllowedExtensions([".mp3", ".wav"])]
        [MaxFileSize(10)]
        public required IFormFile Image { get; set; }
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
    }
}
