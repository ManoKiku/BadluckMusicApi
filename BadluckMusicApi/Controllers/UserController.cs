using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleSavedMusic()
        {
            throw new NotImplementedException();
        }
    }
}
