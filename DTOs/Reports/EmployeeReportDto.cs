namespace TaskManagementApi.DTOs.Reports
{
    public class EmployeeReportDto
    {
        public required string LoginId { get; set; }
        public required string Name { get; set; }
        public int TotalAssigned { get; set; }
        public int CompletedInTime { get; set; }
        public int OverdueCount { get; set; }
        public string RatioCompletedToOverdue { get; set; } = string.Empty;
    }
}