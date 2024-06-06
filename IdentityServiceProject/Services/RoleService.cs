using IdentityDb.UnitOfWork;
using IdentityServiceProject.Dtos;
using IdentityServiceProject.Helpers;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceProject.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService( RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> AddUserRole(UserRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return string.Empty;
            }

            var response = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (response.Succeeded)
            {
                return model.ToString();
            }
            
            return string.Empty;
        }

        public async Task<IEnumerable<string?>> GetRolesAsync()
        {
            List<string?> list = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return list;
        }

        public async Task<bool> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName) || await _roleManager.RoleExistsAsync(roleName))
            {
                return false;
            }

            var response = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (response.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}
