namespace BadluckMusicApi.Models
{
    public class MusicMood
    {
        public int Id { get; set; }
        public int MoodId { get; set; }
        public string MusicId { get; set; }

        public Mood Mood { get; set; }
        public Music Music { get; set; }
    }
}
