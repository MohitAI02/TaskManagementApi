namespace TaskManagementApi.DTOs.Tasks
{
    public class TaskDto
    {
        public int taskId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ActualCompletedDate { get; set; }
        public required string Status { get; set; }
        public required string AssignedTo { get; set; }
        public required string CreatedBy { get; set; }
    }
}