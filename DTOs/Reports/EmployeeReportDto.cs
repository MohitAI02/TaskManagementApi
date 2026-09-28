namespace TaskManagementApi.DTOs.Reports
{
    public class EmployeeReportDto
    {
        public required string loginId { get; set; }
        public required string name { get; set; }
        public int totalAssigned { get; set; }
        public int completedInTime { get; set; }
        public int overdueCount { get; set; }
        public string ratioCompletedToOverdue { get; set; } = string.Empty;
    }
}