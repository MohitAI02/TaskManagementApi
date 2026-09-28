namespace TaskManagementApi.DTOs.Employees
{
    public class EmployeeDto
    {
        public int userId { get; set; }
        public required string loginId { get; set; }
        public required string name { get; set; }
        public required string designation { get; set; }
    }
}