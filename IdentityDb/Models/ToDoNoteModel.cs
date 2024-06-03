using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace IdentityDb.Models
{
    public class ToDoNoteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("IdentityUser")]
        public string UId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }

        [AllowNull]
        public DateTime DeadlineDateTime { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
