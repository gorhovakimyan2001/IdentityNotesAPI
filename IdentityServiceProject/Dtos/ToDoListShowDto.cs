using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServiceProject.Dtos
{
    public class ToDoListShowDto
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime DeadlineDateTime { get; set; }
    }
}
