using BadluckMusicApi.Models.DB;

namespace BadluckMusicApi.Services
{
    public interface IMusicService
    {
        Task<Music> AddMusicAsync(Music music);
        Task<Music?> GetMusicAsync(int id);
        Task<IEnumerable<Music>> GetAllMusicAsync();
        Task<IEnumerable<Music>> GetSortedMusicAsync(IEnumerable<int> hobbyIds, IEnumerable<int> moodIds);
        Task UpdateMusicAsync(Music music);
        Task DeleteMusicAsync(int id);
        Task<IEnumerable<Music>> GetAuthorMusicAsync(int authorId);
        Task<IEnumerable<Music>> GetUserMusicAllAsync(string userId);
    }

}
