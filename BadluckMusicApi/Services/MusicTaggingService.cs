using BadluckMusicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BadluckMusicApi.Services
{
    public class MusicTaggingService : IMusicTaggingService
    {
        private readonly AppDbContext _context;

        public MusicTaggingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMusicHobbyAsync(MusicHobby hobby)
        {
            await _context.MusicHobbies.AddAsync(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task AddMusicHobbiesAsync(IEnumerable<MusicHobby> hobbies)
        {
            await _context.MusicHobbies.AddRangeAsync(hobbies);
            await _context.SaveChangesAsync();
        }

        public async Task AddMusicMoodAsync(MusicMood mood)
        {
            await _context.MusicMoods.AddAsync(mood);
            await _context.SaveChangesAsync();
        }

        public async Task AddMusicMoodsAsync(IEnumerable<MusicMood> moods)
        {
            await _context.MusicMoods.AddRangeAsync(moods);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMusicHobbyAsync(int id)
        {
            var item = await _context.MusicHobbies.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("No music hobby with such key");
            }

            _context.MusicHobbies.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMusicMoodAsync(int id)
        {
            var item = await _context.MusicMoods.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("No music hobby with such key");
            }

            _context.MusicMoods.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
