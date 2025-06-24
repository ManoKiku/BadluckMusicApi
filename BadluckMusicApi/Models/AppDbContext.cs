using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BadluckMusicApi.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<MusicHobby> MusicHobbies{ get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<MusicMood> MusicMoods { get; set; }
        public DbSet<SavedMusic> SavedMusics { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
