using Microsoft.AspNetCore.Identity;

namespace BadluckMusicApi.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<SavedMusic> SavedMusic { get; set; }
    }
}
