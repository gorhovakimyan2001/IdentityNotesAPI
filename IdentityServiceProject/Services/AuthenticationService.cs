using IdentityServiceProject.Dtos;
using IdentityServiceProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
namespace IdentityServiceProject.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        public AuthenticationService(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        public async Task<IdentityResult> Registration(UserRegisterDto newUser)
        {
            var user = new IdentityUser() 
            { 
                Email = newUser.Email,
                UserName = newUser.UserName,
            };

            var responce = await _userManager.CreateAsync(user, newUser.Password);
            return responce;
        }

        public async Task<(SignInResult result, string token)> LogIn(UserLogInDto user)
        {
            var userDb = await _userManager.FindByEmailAsync(user.Email);

            if (userDb == null)
            {
                throw new Exception("User doesn't exist!!!");
            }
            var result = await _signInManager.PasswordSignInAsync(userDb.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var existingToken = await _userManager.GetAuthenticationTokenAsync(userDb, "Default", "login");
                if (existingToken != null)
                {
                    return (result, existingToken);
                }

                string tonkenKeyString = _config.GetSection("AppSettings").GetSection("TokenKey").Value ?? string.Empty;
                var token = TokenHelper.GenerateToken(tonkenKeyString, userDb.UserName ?? string.Empty);
                await _userManager.SetAuthenticationTokenAsync(userDb, "Default", "login", token);
                return (result, token);
            }

            return (result, string.Empty);
        }

        public async Task<string> RefeshToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return  $"There is no user with {userId} ID";
            }

            string tonkenKeyString = _config.GetSection("AppSettings").GetSection("TokenKey").Value ?? string.Empty;
            var token = TokenHelper.GenerateToken(tonkenKeyString, user.UserName ?? string.Empty);
            await _userManager.SetAuthenticationTokenAsync(user, "Default", "login", token);
            return token;
        }
    }
}
