using IdentityServiceProject.Dtos;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(UserRegisterDto newUser)
        {
            var response = await _authenticationService.Registration(newUser);

            if (response == null || !response.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(UserLogInDto user)
        {
            var response = await _authenticationService.LogIn(user);

            if (response.result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, response.token);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([EmailAddress] string email)
        {
            var response = await _authenticationService.RefeshToken(email);

            if (string.IsNullOrEmpty(response))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
