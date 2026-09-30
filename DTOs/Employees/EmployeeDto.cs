namespace TaskManagementApi.DTOs.Employees
{
    public class EmployeeDto
    {
        public int UserId { get; set; }
        public required string LoginId { get; set; }
        public required string Name { get; set; }
        public required string Designation { get; set; }
    }
}