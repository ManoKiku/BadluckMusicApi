using BadluckMusicApi.Models.DB;

namespace BadluckMusicApi.Services
{
    public interface IHobbyService
    {
        Task<Hobby?> GetHobbyAsync(int id);
        Task<IEnumerable<Hobby>> GetAllHobbiesAsync();
        Task<Hobby> AddHobbyAsync(Hobby hobby);
        Task DeleteHobbyAsync(int id);
        Task UpdateHobbyAsync(Hobby hobby);
    }
}
