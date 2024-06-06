using IdentityServiceProject.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServiceProject.Helpers
{
    public class UserHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }

        public async Task<IdentityUser> GetCurrentUser()
        {
            string userName = GetCurrentUserName();
            if (string.IsNullOrEmpty(userName))
            { 
                return null;
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<IdentityUser?> GetUser(string identifier, UserIdentifierEnum e)
        {
            var user = new IdentityUser();

            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            if (e == UserIdentifierEnum.Email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else if (e == UserIdentifierEnum.UserName)
            {
                user = await _userManager.FindByNameAsync(identifier);
            }
            else if (e == UserIdentifierEnum.Id)
            {
                user = await _userManager.FindByIdAsync(identifier);
            }

            return user;
        }
    }
}
