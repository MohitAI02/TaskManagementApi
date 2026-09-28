namespace TaskManagementApi.DTOs.Tasks
{
    public class TaskDto
    {
        public int taskId { get; set; }
        public required string title { get; set; }
        public string? description { get; set; }
        public DateTime createDate { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime? actualCompletedDate { get; set; }
        public required string status { get; set; }
        public required string assignedTo { get; set; }
        public required string createdBy { get; set; }
    }
}