using BadluckMusicApi.Helpers;
using BadluckMusicApi.Models.ViewModels;
using BadluckMusicApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadluckMusicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (!result)
            {
                _logger.LogWarning("Login failed for {Username}", model.Username);
                return ApiResponseHelper.BadRequest("User with such username already exists");
            }

            _logger.LogInformation("User {Username} logged in", model.Username);
            return ApiResponseHelper.Success("Login success");
        }


        [HttpPost("registration")]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.ValidationError(ModelState);
            }

            var result = await _authService.RegisterAsync(model);

            if (!result)
            {
                _logger.LogWarning("Registration failed for {Name}", model.Name);
                return ApiResponseHelper.BadRequest("Registration data is incorrect");
            }

            _logger.LogInformation("User {Name} registered", model.Name);
            return ApiResponseHelper.Success("Registration success");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return ApiResponseHelper.Success("Logout success");
        }
    }
}
