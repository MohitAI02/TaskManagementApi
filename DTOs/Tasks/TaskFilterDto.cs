namespace TaskManagementApi.DTOs.Tasks
{
    public class TaskFilterDto
    {
        public string? status { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}