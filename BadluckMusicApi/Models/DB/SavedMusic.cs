namespace BadluckMusicApi.Models.DB
{
    public class SavedMusic
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MusicId { get; set; }

        public User User { get; set; }
        public Music Music { get; set; }
    }
}
