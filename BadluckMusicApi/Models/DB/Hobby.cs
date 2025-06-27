namespace BadluckMusicApi.Models.DB
{
    public class Hobby
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public IEnumerable<MusicHobby> MusicHobbies { get; set; }
    }
}
