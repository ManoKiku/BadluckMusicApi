using BadluckMusicApi.Models.DB;

namespace BadluckMusicApi.Services
{
    public interface IAuthorService
    {
        Task<Author> AddAuthorAsync(Author author);
        Task<Author?> GetAuthorAsync(int id);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
    }
}
