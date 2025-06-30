using BadluckMusicApi.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace BadluckMusicApi.Services
{
    public class TagService : IHobbyService, IMoodService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Hobby> AddHobbyAsync(Hobby hobby)
        {
            var result = await _context.Hobbies.AddAsync(hobby);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Mood> AddMoodAsync(Mood mood)
        {
            var result = await _context.Moods.AddAsync(mood);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteHobbyAsync(int id)
        {
            var item = await _context.Hobbies.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("No hobby with such key");
            }

            _context.Hobbies.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMoodAsync(int id)
        {
            var item = await _context.Moods.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("No mood with such key");
            }

            _context.Moods.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hobby>> GetAllHobbiesAsync()
        {
            return await _context.Hobbies.ToListAsync();
        }

        public async Task<Hobby?> GetHobbyAsync(int id)
        {
            return await _context.Hobbies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Mood?> GetMoodAsync(int id)
        {
            return await _context.Moods.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<IEnumerable<Mood>> GetAllMoodsAsync()
        {
            return await _context.Moods.ToListAsync();
        }

        public async Task UpdateHobbyAsync(Hobby hobby)
        {
            _context.Hobbies.Update(hobby);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMoodAsync(Mood mood)
        {
            _context.Moods.Update(mood);
            await _context.SaveChangesAsync();
        }
    }
}
