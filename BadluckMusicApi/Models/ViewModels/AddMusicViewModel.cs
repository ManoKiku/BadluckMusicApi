using BadluckMusicApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BadluckMusicApi.Models.ViewModels
{
    public class AddMusicViewModel
    {
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [AllowedExtensions([".mp3", ".wav"])]
        [MaxFileSize(10)]
        public required IFormFile MusicFile { get; set; }

        [Required]
        [AllowedExtensions([".jpg", ".png"])]
        [MaxFileSize(5)]
        public required IFormFile CoverFile { get; set; }

        [Required]
        [AuthorExists]
        public required int AuthorId { get; set; }

        public List<int>? HobbyIds { get; set; }
        public List<int>? MoodIds { get; set; }
    }
}
