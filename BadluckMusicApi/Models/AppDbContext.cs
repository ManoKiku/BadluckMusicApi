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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>()
                .HasMany(a => a.Musics)
                .WithOne(m => m.Author)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Music>()
                .HasMany(m => m.MusicHobbys)
                .WithOne(mh => mh.Music)
                .HasForeignKey(mh => mh.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Music>()
                .HasMany(m => m.MusicMoods)
                .WithOne(mm => mm.Music)
                .HasForeignKey(mm => mm.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hobby>()
                .HasMany(h => h.MusicHobbies)
                .WithOne(mh => mh.Hobby)
                .HasForeignKey(mh => mh.HobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Mood>()
                .HasMany(m => m.MusicMoods)
                .WithOne(mm => mm.Mood)
                .HasForeignKey(mm => mm.MoodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.SavedMusic)
                .WithOne(sm => sm.User)
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SavedMusic>()
                .HasOne(sm => sm.Music)
                .WithMany()
                .HasForeignKey(sm => sm.MusicId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
