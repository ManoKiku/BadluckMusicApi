namespace BadluckMusicApi.Models
{
    public class MusicHobby
    {
        public int Id { get; set; }
        public int HobbyId { get; set; }
        public string MusicId { get; set; }
         
        public Hobby Hobby {  get; set; } 
        public Music Music { get; set; }
    }
}
