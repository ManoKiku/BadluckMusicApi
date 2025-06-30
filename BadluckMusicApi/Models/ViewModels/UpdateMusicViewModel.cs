using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class UpdateMusicViewModel
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [AllowedExtensions([".mp3", ".wav"])]
        [MaxFileSize(10)]
        public IFormFile? MusicFile { get; set; }

        [AllowedExtensions([".jpg", ".png", ".webp"])]
        [MaxFileSize(5)]
        public IFormFile? CoverFile { get; set; }

        [AuthorExists]
        public int? AuthorId { get; set; }
    }
}
