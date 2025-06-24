using BadluckMusicApi.Models;

namespace BadluckMusicApi.Services
{
    public interface IMusicTaggingService
    {
        Task AddMusicHobbyAsync(MusicHobby hobby);
        Task AddMusicMoodAsync(MusicMood mood);
        Task DeleteMusicHobbyAsync(int id);
        Task DeleteMusicMoodAsync(int id);
    }
}
