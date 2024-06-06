using IdentityServiceProject.Dtos;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityProject.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var list = await _roleService.GetRolesAsync();

            if (list.Any())
            {
                return StatusCode(StatusCodes.Status200OK, list);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("UserRole")]
        public async Task<IActionResult> AddRoleToUser(UserRoleDto model)
        {
            var response = await _roleService.AddUserRole(model);

            if (string.IsNullOrEmpty(response))
            {
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var response = await _roleService.AddRole(roleName);
            if (!response)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
