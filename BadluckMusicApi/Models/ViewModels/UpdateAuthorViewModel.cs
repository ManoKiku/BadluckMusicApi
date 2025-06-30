using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class UpdateAuthorViewModel
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [AllowedExtensions([".jpg", ".png"])]
        [MaxFileSize(5)]
        public IFormFile? ImageFile { get; set; }
    }
}
