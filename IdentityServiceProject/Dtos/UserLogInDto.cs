using System.ComponentModel.DataAnnotations;

namespace IdentityServiceProject.Dtos
{
    public class UserLogInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
