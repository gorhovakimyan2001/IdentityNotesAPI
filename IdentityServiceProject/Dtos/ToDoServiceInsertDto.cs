using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServiceProject.Dtos
{
    public class ToDoServiceInsertDto
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DeadlineDateTime { get; set; }
    }
}
