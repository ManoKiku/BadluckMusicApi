using BadluckMusicApi.Models;

namespace BadluckMusicApi.Services
{
    public interface IMusicTaggingService
    {
        Task AddMusicHobbyAsync(MusicHobby hobby);
        Task AddMusicHobbiesAsync(IEnumerable<MusicHobby> hobby);
        Task AddMusicMoodAsync(MusicMood mood);
        Task AddMusicMoodsAsync(IEnumerable<MusicMood> mood);

        Task DeleteMusicHobbyAsync(int id);
        Task DeleteMusicMoodAsync(int id);
    }
}
