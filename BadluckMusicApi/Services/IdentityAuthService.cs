using BadluckMusicApi.Models.DB;
using BadluckMusicApi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BadluckMusicApi.Services
{
    public class IdentityAuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityAuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterAsync(RegistrationViewModel model)
        {
            var user = new User { UserName = model.Name, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);

            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            return result.Succeeded;
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();
    }
}
