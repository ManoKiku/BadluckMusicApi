namespace BadluckMusicApi.Models
{
    public class Mood
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<MusicMood> MusicMoods { get; set; }
    }
}
