using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
