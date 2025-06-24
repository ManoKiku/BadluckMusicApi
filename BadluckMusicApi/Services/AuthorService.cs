using BadluckMusicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BadluckMusicApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            var result = _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
                throw new KeyNotFoundException("No hobby with such key");

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public Task<Author?> GetAuthorAsync(int id)
        {
            return _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }
    }
}
