using BadluckMusicApi.Models.ViewModels;

namespace BadluckMusicApi.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationViewModel model);
        Task<bool> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
