using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Models
{
    public class TaskItem
    {
        [Key]
        [Column("task_id")]
        public int TaskId { get; set; }

        [Required]
        [Column("title")]
        public required string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Column("due_date")]
        public DateTime DueDate { get; set; }
        [Column("actual_completed_date")]
        public DateTime? ActualCompleted_date { get; set; }

        [Required]
        [Column("status")]
        public required string Status { get; set; }

        [Required]
        [Column("assigned_to")]
        public required string AssignedTo { get; set; }

        [Required]
        [Column("created_by")]
        public required string CreatedDy { get; set; }
    }
}