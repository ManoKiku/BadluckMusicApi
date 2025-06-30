using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class AddAuthorViewModel
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }
        [Required]
        [AllowedExtensions([".jpg", ".png", ".webp"])]
        [MaxFileSize(5)]
        public required IFormFile Image { get; set; }
    }
}
