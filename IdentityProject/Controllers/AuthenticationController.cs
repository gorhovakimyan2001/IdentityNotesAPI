using IdentityServiceProject.Dtos;
using IdentityServiceProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("Registration")]
        public IResult Registration(UserRegisterDto newUser)
        {
            var response = _authenticationService.Registration(newUser);
            var result = response.Result;
            return Results.Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public IResult LogIn(UserLogInDto user)
        {
            var response = _authenticationService.LogIn(user);

            if (response.Result.result.Succeeded)
            {
                return Results.Ok(response.Result.token);
            }
            return Results.NotFound(user);
        }

        [HttpPost("RefreshToken")]
        public IResult RefreshToken(string userId)
        {
            var response = _authenticationService.RefeshToken(userId);
            return Results.Ok(response.Result);
        }
    }
}
