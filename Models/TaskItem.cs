using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models
{
    public class TaskItem
    {
        [Key]
        public int task_id { get; set; }

        [Required]
        public required string title { get; set; }

        public string? description { get; set; }

        [Required]
        public DateTime create_date { get; set; }

        [Required]
        public DateTime due_date { get; set; }

        public DateTime? actual_completed_date { get; set; }

        [Required]
        public required string status { get; set; }

        [Required]
        public required string assigned_to { get; set; }

        [Required]
        public required string created_by { get; set; }
    }
}