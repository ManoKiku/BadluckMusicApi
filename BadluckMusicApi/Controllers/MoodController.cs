using BadluckMusicApi.Helpers;
using BadluckMusicApi.Models.DB;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Mvc;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class MoodController : Controller
    {
        private readonly IMoodService _moodService;
        private readonly ILogger<MoodController> _logger;

        public MoodController(IMoodService moodService, ILogger<MoodController> logger)        {
            _moodService = moodService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMoodsAsync()
        {
            try
            {
                var moods = await _moodService.GetAllMoodsAsync();

                if(moods == null || !moods.Any())
                {
                    return ApiResponseHelper.BadRequest("No moods was founded");
                }

                return ApiResponseHelper.Success("Moods was fetched", new
                {
                    Moods = moods
                });
            }
            catch(Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching moods");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMoodAsync(int id)
        {
            try
            {
                var mood = await _moodService.GetMoodAsync(id);

                if (mood == null)
                {
                    return ApiResponseHelper.BadRequest("No mood with such id");
                }

                return ApiResponseHelper.Success("Mood was fetched", new
                {
                    Mood = mood
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching mood");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMoodAsync(string name)
        {
            try
            {
                var mood = new Mood
                {
                    Name = name
                };

                mood = await _moodService.AddMoodAsync(mood);

                return ApiResponseHelper.Success("Mood was added", new
                {
                    MoodId = mood.Id
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while adding mood");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoodAsync(int id)
        {
            try
            {
                var mood = await _moodService.GetMoodAsync(id);

                if (mood == null)
                {
                    return ApiResponseHelper.BadRequest("No mood with such id");
                }

                await _moodService.DeleteMoodAsync(id);

                return ApiResponseHelper.Success("Mood was deleted");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while deleting mood");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMoodAsync(int id, string name)
        {
            try
            {
                var mood = await _moodService.GetMoodAsync(id);

                if (mood == null)
                {
                    return ApiResponseHelper.BadRequest("No mood with such id");
                }

                mood.Name = name;

                await _moodService.UpdateMoodAsync(mood);

                return ApiResponseHelper.Success("Mood was updated");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while updating mood");
            }
        }
    }
}
