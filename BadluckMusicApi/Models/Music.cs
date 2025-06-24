namespace BadluckMusicApi.Models
{
    public class Music
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MusicPath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int AuthorId { get; set; }

        public Author Author { get; set; }
        public IEnumerable<MusicHobby> MusicHobbys { get; set; }
        public IEnumerable<MusicMood> MusicMoods { get; set; }

    }
}
