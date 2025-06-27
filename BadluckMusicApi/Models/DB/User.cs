using Microsoft.AspNetCore.Identity;

namespace BadluckMusicApi.Models.DB
{
    public class User : IdentityUser
    {
        public IEnumerable<SavedMusic> SavedMusic { get; set; }
    }
}
