using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServiceProject.Dtos
{
    public class ToDoUpdateDto
    {
        [Required]
        public int Id { get; set; }

        public bool IsDone { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DeadlineDateTime { get; set; }
    }
}
