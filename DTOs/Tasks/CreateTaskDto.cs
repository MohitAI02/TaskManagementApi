using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public required string AssignedTo { get; set; }
    }
}