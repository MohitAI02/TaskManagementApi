namespace TaskManagementApi.DTOs.Tasks
{
    public class TaskFilterDto
    {
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}