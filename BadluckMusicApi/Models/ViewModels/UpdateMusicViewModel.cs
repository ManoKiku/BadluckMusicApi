using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class UpdateMusicViewModel
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [AllowedExtensions(["mp3", "wav"])]
        [MaxFileSize(10 * 1024 * 1024)]
        public IFormFile? MusicFile { get; set; }

        [AllowedExtensions(["jpg", "png"])]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? CoverFile { get; set; }

        [AuthorExists]
        public int? AuthorId { get; set; }
    }
}
