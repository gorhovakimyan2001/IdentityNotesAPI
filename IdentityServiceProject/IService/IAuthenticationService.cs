using IdentityServiceProject.Dtos;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceProject.IService
{
    public interface IAuthenticationService
    {
        public Task<IdentityResult> Registration(UserRegisterDto newUser);

        public Task<(SignInResult result, string token)> LogIn(UserLogInDto user);

        public Task<string> RefeshToken(string email);
    }
}
