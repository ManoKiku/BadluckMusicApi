namespace BadluckMusicApi.Models
{
    public class MusicMood
    {
        public int Id { get; set; }
        public int MoodId { get; set; }
        public int MusicId { get; set; }

        public Mood Mood { get; set; }
        public Music Music { get; set; }
    }
}
