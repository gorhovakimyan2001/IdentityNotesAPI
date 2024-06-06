using System.ComponentModel.DataAnnotations;

namespace IdentityServiceProject.Dtos
{
    public class UserRoleDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string RoleName { get; set; }

        public override string ToString()
        {
            return Email + " ---> " + RoleName;
        }
    }
}
