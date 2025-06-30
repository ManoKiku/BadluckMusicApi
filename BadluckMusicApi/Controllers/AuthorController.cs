using BadluckMusicApi.Helpers;
using BadluckMusicApi.Models.DB;
using BadluckMusicApi.Models.ViewModels;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace BadluckMusicApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorService _authorService;
        private readonly IFileService _fileService;
        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService, IFileService fileService) 
        { 
            _logger = logger;
            _authorService = authorService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            try
            {
                var authors = await _authorService.GetAllAuthorsAsync();

                if (authors == null || !authors.Any())
                {
                    return ApiResponseHelper.BadRequest("No authors");
                }

                return ApiResponseHelper.Success("Authors was fetched", new
                {
                    Authors = authors
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching music");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorByIdAsync(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorAsync(id);

                if (author == null)
                {
                    return ApiResponseHelper.BadRequest("No author with such id");
                }

                return ApiResponseHelper.Success("Author was fetched", new
                {
                    Author = author
                });
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while fetching music");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAuthorAsync(AddAuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            string imagePath = $"data/authors/{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
            Author? author = null;

            try
            {
                if (!await _fileService.UploadFileAsync(model.Image.OpenReadStream(), imagePath))
                {
                    throw new IOException("Error while uploading image file");
                }

                author = new Author
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImagePath = imagePath
                };

                author = await _authorService.AddAuthorAsync(author);

                return ApiResponseHelper.Success("New author was added successfuly", new
                {
                    AuthorId = author.Id
                });
            }
            catch (Exception ex)
            {
                var rollbackTasks = new List<Task>();

                if (author?.Id > 0)
                {
                    rollbackTasks.Add(_authorService.DeleteAuthorAsync(author.Id));
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

                return ApiResponseHelper.ServerError(_logger, ex, "Error while adding author");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthorAsync(int id)
        {
            var author = await _authorService.GetAuthorAsync(id);

            if(author == null)
            {
                return ApiResponseHelper.BadRequest("No author with such id");
            }

            try
            {
                await _fileService.DeleteFileAsync(author.ImagePath);
                await _authorService.DeleteAuthorAsync(id);
                return ApiResponseHelper.Success("Author was deleted");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while deleting author");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAuthorAsync(int id, UpdateAuthorViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            var author = await _authorService.GetAuthorAsync(id);

            if(author == null)
            {
                return ApiResponseHelper.BadRequest("No author with such id");
            }

            string? imagePath = null;

            if (model.ImageFile != null)
                imagePath = $"data/authors/{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";

            try
            {
                await _fileService.DeleteFileAsync(author.ImagePath);

                if (imagePath != null)
                {
                    if(!await _fileService.UploadFileAsync(model.ImageFile!.OpenReadStream(), imagePath))
                        throw new IOException("Error while uploading music file");
                    
                    author.ImagePath = imagePath;
                }
                
                if (model.Name != null)
                    author.Name = model.Name;

                if (model.Description != null)
                    author.Description = author.Description;

                await _authorService.UpdateAuthorAsync(author);

                return ApiResponseHelper.Success("Author was updated");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.ServerError(_logger, ex, "Error while updating author");
            }
        }
    }
}
