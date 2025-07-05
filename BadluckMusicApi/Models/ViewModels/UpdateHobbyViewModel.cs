using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class UpdateHobbyViewModel
    {
        [AllowedExtensions([".mp3", ".wav"])]
        [MaxFileSize(10)]
        public IFormFile? ImageFile { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
