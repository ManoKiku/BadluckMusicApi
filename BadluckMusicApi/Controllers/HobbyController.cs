using BadluckMusicApi.Helpers;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class HobbyController : Controller
    {
        private readonly ILogger<HobbyController> _logger;
        private readonly IHobbyService _hobbyService;

        public HobbyController(ILogger<HobbyController> logger, IHobbyService hobbyService) 
        { 
            _logger = logger;
            _hobbyService = hobbyService;
        }

        public async Task<IActionResult> GetHobbiesAsync()
        {
            try 
            {
                var hobbies = await _hobbyService.GetAllHobbiesAsync();

                if (hobbies == null || !hobbies.Any())
                {
                    return ApiResponseHelper.BadRequest("No hobby with such id");
                }

                return ApiResponseHelper.Success("Hobbies was fetched", new
                {
                    Hobbies = hobbies
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching music");
            }
        }
    }
}
