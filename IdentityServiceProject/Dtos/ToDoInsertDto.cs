using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IdentityServiceProject.Dtos
{
    public class ToDoInsertDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [AllowNull]
        public DateTime DeadlineDateTime { get; set; }
    }
}
