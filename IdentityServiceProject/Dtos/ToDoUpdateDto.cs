using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
