using BadluckMusicApi.Models;
using BadluckMusicApi.Services;
using BadluckMusicApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IMusicTaggingService _taggingService;
        private readonly ILogger<MusicController> _logger;
        private readonly IFileService _fileService;

        public MusicController(IMusicService musicService,
            IMusicTaggingService taggingService, 
            ILogger<MusicController> logger,
            IFileService fileService) 
        {
            _musicService = musicService;
            _taggingService = taggingService;
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMusicAsync()
        {
            try
            {
                var music = await _musicService.GetAllMusicAsync();

                if (music == null || !music.Any())
                {
                    return NotFound(new
                    {
                        Status = "error",
                        Message = "No music"
                    });
                }

                return Ok(new
                {
                    Status = "success",
                    Message = "Music was fetched",
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

        [HttpGet("filter")]
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
                    Message = "Music was fetched",
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicAsync(int id)
        {
            try
            {
                var music = await _musicService.GetMusicAsync(id);

                if (music == null)
                {
                    return NotFound(new
                    {
                        Status = "error",
                        Message = "No music with such id"
                    });
                }

                return Ok(new
                {
                    Status = "success",
                    Message = "Music was fetched",
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMusicAsync(AddMusicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "There is validation errors in model",
                    Errors = ModelState.ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            string musicPath = $"data/music/{Guid.NewGuid()}{Path.GetExtension(model.MusicFile.FileName)}";
            string coverPath = $"data/covers/{Guid.NewGuid()}{Path.GetExtension(model.CoverFile.FileName)}";
            Music? music = null;

            try
            {
                if(!await _fileService.UploadFileAsync(model.MusicFile.OpenReadStream(), musicPath))
                {
                    throw new IOException("Error while uploading music file");
                }

                if (!await _fileService.UploadFileAsync(model.CoverFile.OpenReadStream(), coverPath))
                {
                    throw new IOException("Error while uploading cover file");
                }

                music = new Music
                {
                    AuthorId = model.AuthorId,
                    ImagePath = coverPath,
                    MusicPath = musicPath,
                    Title = model.Title
                };

                music = await _musicService.AddMusicAsync(music);

                if(model.HobbyIds != null && model.HobbyIds.Any())
                    await _taggingService.AddMusicHobbiesAsync(model.HobbyIds.Select(x => new MusicHobby { HobbyId = x, MusicId = music.Id }));

                if (model.MoodIds != null && model.MoodIds.Any())
                    await _taggingService.AddMusicMoodsAsync(model.MoodIds.Select(x => new MusicMood { MoodId = x, MusicId = music.Id }));

                return Ok(new
                {
                    Status = "success",
                    Message = "New music was added",
                    MusicId = music.Id
                });
            }
            catch(Exception ex)
            {
                var rollbackTasks = new List<Task>();

                if (music?.Id > 0)
                {
                    rollbackTasks.Add(_musicService.DeleteMusicAsync(music.Id));
                }

                if (!string.IsNullOrEmpty(musicPath))
                {
                    rollbackTasks.Add(_fileService.DeleteFileAsync(musicPath));
                }

                if (!string.IsNullOrEmpty(coverPath))
                {
                    rollbackTasks.Add(_fileService.DeleteFileAsync(coverPath));
                }

                try
                {
                    await Task.WhenAll(rollbackTasks);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError(rollbackEx, "Error during rollback operations");
                }

                _logger.LogWarning(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "error",
                        Message = "Error while adding new music"
                    });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMusicAsync(int id)
        {
            var music = await _musicService.GetMusicAsync(id);

            if (music == null)
                return BadRequest(new
                {
                    Status = "error",
                    Message = "No music with such id"
                });

            try
            {
                await _fileService.DeleteFileAsync(music.MusicPath);
                await _fileService.DeleteFileAsync(music.MusicPath);
                await _musicService.DeleteMusicAsync(id);

                return Ok(new
                {
                    Status = "success",
                    Message = "Music was deleted"
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "error",
                        Message = "Error while deleting music"
                    });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMusicAsync(int id, UpdateMusicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = "error",
                    Message = "There is validation errors in model",
                    Errors = ModelState.ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            var music = await _musicService.GetMusicAsync(id);

            if (music == null)
                return BadRequest(new
                {
                    Status = "error",
                    Message = "No music with such id"
                });

            string? musicPath = null;
            string? coverPath = null;

            if(model.MusicFile != null)
                musicPath = $"data/music/{Guid.NewGuid()}{Path.GetExtension(model.MusicFile.FileName)}";

            if(model.CoverFile != null)
                coverPath = $"data/covers/{Guid.NewGuid()}{Path.GetExtension(model.CoverFile.FileName)}";

            try
            {
                await _fileService.DeleteFileAsync(music.MusicPath);
                await _fileService.DeleteFileAsync(music.MusicPath);

                if (musicPath != null && !await _fileService.UploadFileAsync(model.MusicFile!.OpenReadStream(), musicPath))
                {
                    music.MusicPath = musicPath;
                    throw new IOException("Error while uploading music file");
                }

                if (coverPath != null && !await _fileService.UploadFileAsync(model.CoverFile!.OpenReadStream(), coverPath))
                {
                    music.ImagePath = coverPath;
                    throw new IOException("Error while uploading cover file");
                }

                if(model.Title != null)
                    music.Title = model.Title;

                if(model.AuthorId != null)
                    music.AuthorId = (int)model.AuthorId;

                await _musicService.UpdateMusicAsync(music);

                return Ok(new
                {
                    Status = "success",
                    Message = "Music was updated"
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "error",
                        Message = "Error while updating music"
                    });
            }
        }
    }
}
