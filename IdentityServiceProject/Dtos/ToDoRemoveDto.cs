using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityServiceProject.Dtos
{
    public class ToDoRemoveDto
    {
        [JsonIgnore]
        public string UserName { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
