using BadluckMusicApi.Models;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly ILogger<MusicController> _logger;

        public MusicController(IMusicService musicService, ILogger<MusicController> logger) 
        { 
            _musicService = musicService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMusicAsync([FromQuery] List<int> hobbyIds, [FromQuery] List<int> moodIds)
        {
            try
            {
                var music = await _musicService.GetSortedMusicAsync(hobbyIds, moodIds);

                if (music.Count() == 0)
                {
                    return NotFound(new
                    {
                        Status = "error",
                        Message = "No music with such parameters"
                    });
                }

                return Ok(new
                {
                    Status = "success",
                    Music = music
                });

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "error",
                        Message = "Error while fetching music"
                    });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMusicAsync()
        {
            throw new NotImplementedException();
        }
    }
}
