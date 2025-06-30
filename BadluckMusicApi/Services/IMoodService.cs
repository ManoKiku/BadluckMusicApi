using BadluckMusicApi.Models.DB;

namespace BadluckMusicApi.Services
{
    public interface IMoodService
    {
        Task<Mood?> GetMoodAsync(int id);
        Task<IEnumerable<Mood>> GetAllMoodsAsync();
        Task<Mood> AddMoodAsync(Mood mood);
        Task DeleteMoodAsync(int id);
        Task UpdateMoodAsync(Mood mood);
    }
}
