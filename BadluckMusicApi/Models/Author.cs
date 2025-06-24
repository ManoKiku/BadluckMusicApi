namespace BadluckMusicApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public IEnumerable<Music> Musics { get; set; }
    }
}
