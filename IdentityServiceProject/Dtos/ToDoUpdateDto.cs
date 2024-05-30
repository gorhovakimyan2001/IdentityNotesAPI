﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityServiceProject.Dtos
{
    public class ToDoUpdateDto : ToDoBase
    {
        [Required]
        public int Id { get; set; }

        public bool IsDone { get; set; }
    }
}
