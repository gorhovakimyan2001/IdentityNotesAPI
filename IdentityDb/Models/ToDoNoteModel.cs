using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDb.Models
{
    public class ToDoNoteModel
    {
        public int Id { get; set; } 

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }

        [AllowNull]
        public DateTime DeadlineDateTime { get; set; }
    }
}
