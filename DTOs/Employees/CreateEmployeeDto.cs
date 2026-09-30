using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Designation { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}