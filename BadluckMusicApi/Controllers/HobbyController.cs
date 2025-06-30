using BadluckMusicApi.Helpers;
using BadluckMusicApi.Models.DB;
using BadluckMusicApi.Models.ViewModels;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class HobbyController : Controller
    {
        private readonly ILogger<HobbyController> _logger;
        private readonly IHobbyService _hobbyService;
        private readonly IFileService _fileService;

        public HobbyController(ILogger<HobbyController> logger, IHobbyService hobbyService, IFileService fileService) 
        { 
            _logger = logger;
            _hobbyService = hobbyService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHobbiesAsync()
        {
            try 
            {
                var hobbies = await _hobbyService.GetAllHobbiesAsync();

                if (hobbies == null || !hobbies.Any())
                {
                    return ApiResponseHelper.BadRequest("No hobbies was founded");
                }

                return ApiResponseHelper.Success("Hobbies was fetched", new
                {
                    Hobbies = hobbies
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching hobbies");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHobbyAsync(int id)
        {
            try
            {
                var hobby = await _hobbyService.GetHobbyAsync(id);

                if (hobby == null)
                {
                    return ApiResponseHelper.BadRequest("No hobby with such id");
                }

                return ApiResponseHelper.Success("Hobby was fetched", new
                {
                    Hobby = hobby
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching hobby");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHobbyAsync(AddHobbyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            string imagePath = $"/data/hobbies/{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
            Hobby? hobby = null;

            try
            {
                if (!await _fileService.UploadFileAsync(model.Image.OpenReadStream(), imagePath))
                {
                    throw new IOException("Error while uploading image file");
                }

                hobby = new Hobby
                {
                    Name = model.Name,
                    ImagePath = imagePath
                };

                hobby = await _hobbyService.AddHobbyAsync(hobby);

                return ApiResponseHelper.Success("New hobby was added successfuly", new
                {
                    HobbyId = hobby.Id
                });
            }
            catch (Exception ex)
            {
                var rollbackTasks = new List<Task>();

                if (hobby?.Id > 0)
                {
                    rollbackTasks.Add(_hobbyService.DeleteHobbyAsync(hobby.Id));
                }

                if (!string.IsNullOrEmpty(imagePath))
                {
                    rollbackTasks.Add(_fileService.DeleteFileAsync(imagePath));
                }

                try
                {
                    await Task.WhenAll(rollbackTasks);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError(rollbackEx, "Error during rollback operations");
                }

                return ApiResponseHelper.ServerError(_logger, ex, "Error while adding hobby");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHobbyAsync(int id)
        {
            try
            {
                var hobby = await _hobbyService.GetHobbyAsync(id);

                if(hobby == null)
                {
                    return ApiResponseHelper.BadRequest("No hobby with such id");
                }

                await _fileService.DeleteFileAsync(hobby.ImagePath);
                await _hobbyService.DeleteHobbyAsync(id);
                return ApiResponseHelper.Success("Hobby was deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while deleting hobby");
            }
        }

        [HttpPut("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHobbyAsync(int id, UpdateHobbyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            try
            {
                var hobby = await _hobbyService.GetHobbyAsync(id);

                if (hobby == null)
                {
                    return ApiResponseHelper.BadRequest("No hobby with such id");
                }

                if(model.Name != null)
                {
                    hobby.Name = model.Name;
                }

                if (model.ImageFile != null)
                {
                    string hobbyPath = $"/data/hobbies/{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";
                    await _fileService.UploadFileAsync(model.ImageFile.OpenReadStream(), hobbyPath);
                    await _fileService.DeleteFileAsync(hobby.ImagePath);
                    hobby.ImagePath = hobbyPath;
                }

                await _hobbyService.UpdateHobbyAsync(hobby);
                return ApiResponseHelper.Success("Hobby was updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while updating hobby");
            }
        }
    }
}
