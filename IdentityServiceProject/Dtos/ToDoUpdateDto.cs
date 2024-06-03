using System.ComponentModel.DataAnnotations;

namespace IdentityServiceProject.Dtos
{
    public class ToDoUpdateDto : ToDoBase
    {
        [Required]
        public int Id { get; set; }

        public bool IsDone { get; set; }
    }
}
