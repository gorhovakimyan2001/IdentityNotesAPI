using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IdentityServiceProject.Dtos
{
    public class ToDoBase
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DeadlineDateTime { get; set; }

        [JsonIgnore]
        public virtual string? UserName { get; set; }
    }
}
