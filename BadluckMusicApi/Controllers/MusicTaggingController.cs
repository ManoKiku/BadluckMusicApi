using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class MusicTaggingController : Controller
    {
        private readonly IMusicTaggingService _musicTaggingService;
        private readonly ILogger<MusicTaggingController> _logger;

        public MusicTaggingController(IMusicTaggingService musicTaggingService, ILogger<MusicTaggingController> logger)
        {
            _musicTaggingService = musicTaggingService;
            _logger = logger;
        }
    }
}
