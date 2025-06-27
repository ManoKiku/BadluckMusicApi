using BadluckMusicApi.Models.DB;

namespace BadluckMusicApi.Services
{
    public interface ITagService
    {
        Task<Hobby> AddHobbyAsync(Hobby hobby);
        Task<Mood> AddMoodAsync(Mood mood);
        Task DeleteHobbyAsync(int id);
        Task DeleteMoodAsync(int id);
        Task UpdateHobbyAsync(Hobby hobby);
        Task UpdateMoodAsync(Mood mood);
    }
}
