using BadluckMusicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BadluckMusicApi.Services
{
    public class MusicService : IMusicService
    {
        private readonly AppDbContext _context;

        public MusicService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Music> AddMusicAsync(Music music)
        {
            var result = await _context.Musics.AddAsync(music);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteMusicAsync(int id)
        {
            var item = await _context.Musics.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                throw new KeyNotFoundException("No music with such key");

            _context.Musics.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Music>> GetAllMusicAsync()
        {
            return await _context.Musics.ToListAsync();
        }

        public async Task<IEnumerable<Music>> GetAuthorMusicAsync(int authorId)
        {
            var music = _context.Musics.Where(x => x.AuthorId == authorId);

            return await music.ToListAsync();
        }

        public async Task<Music?> GetMusicAsync(int id)
        {
            return await _context.Musics.FirstOrDefaultAsync(x => x.Id == id);    
        }

        public async Task<IEnumerable<Music>> GetSortedMusicAsync(IEnumerable<int> hobbyIds, IEnumerable<int> moodIds)
        {
            var query = _context.Musics.AsQueryable();

            if (hobbyIds?.Any() == true)
                query = query.Where(m => m.MusicHobbys.Any(mh => hobbyIds.Contains(mh.HobbyId)));
            

            if (moodIds?.Any() == true)
                query = query.Where(m => m.MusicMoods.Any(mm => moodIds.Contains(mm.MoodId)));

            return await query
                .Include(x => x.MusicMoods)
                .Include(x => x.MusicHobbys)
                .ToListAsync();
        }

        public async Task<IEnumerable<Music>> GetUserMusicAllAsync(string userId)
        {
            return await _context.SavedMusics
                .Where(x => x.UserId == userId)
                .Select(x => x.Music)
                .ToListAsync();
        }

        public async Task UpdateMusicAsync(Music music)
        {
            _context.Musics.Update(music);
            await _context.SaveChangesAsync();
        }
    }
}
