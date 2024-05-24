using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
