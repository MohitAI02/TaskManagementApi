using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        public required string title { get; set; }

        public string? description { get; set; }

        [Required]
        public DateTime dueDate { get; set; }

        [Required]
        public required string assignedTo { get; set; }
    }
}