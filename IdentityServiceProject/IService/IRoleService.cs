using IdentityServiceProject.Dtos;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceProject.IService
{
    public interface IRoleService
    {
        public Task<IEnumerable<string?>> GetRolesAsync();

        public Task<string> AddUserRole(UserRoleDto model);

        public Task<bool> AddRole(string roleName);
    }
}
