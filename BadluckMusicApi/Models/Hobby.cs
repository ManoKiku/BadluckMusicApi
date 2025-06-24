namespace BadluckMusicApi.Models
{
    public class Hobby
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public IEnumerable<MusicHobby> MusicHobbys { get; set; }
    }
}
