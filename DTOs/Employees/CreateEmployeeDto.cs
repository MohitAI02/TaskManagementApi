using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        [Required]
        public required string name { get; set; }

        [Required]
        public required string designation { get; set; }

        [Required]
        [EmailAddress]
        public required string email { get; set; }
    }
}